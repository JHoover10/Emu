using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Emu;

public class EmuService
{
    private List<GameData> gameData;
    private List<string> programs;
    private List<string> updates;
    private readonly TaskScheduler taskScheduler;

    public EmuService()
    {
        taskScheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, 5).ConcurrentScheduler;
    }

    public int GetProgressMaxValue()
    {
        GetProgamsAndUpdates();
        gameData = GetGameData();

        var games = gameData.Select(x => x.Name).ToList();
        var installedGames = new List<GameData>();

        foreach (var program in programs)
        {
            var comparer = (string game, string installedProgram) =>
            {
                var tempGame = game.ToLower();
                var tempProgram = installedProgram.ToLower();

                return tempGame == tempProgram;
            };

            if (games.Any(x => comparer(x, program)))
            {
                installedGames.Add(gameData.Single(x => comparer(x.Name, program)));
            }
        }

        gameData = installedGames;

        return 6 + gameData.SelectMany(x => x.ConfigPaths).Count() + gameData.SelectMany(x => x.SaveGameDataPaths).Count();
    }

    public Task BackupDefaultFolders(string path, IProgress<int> progress)
    {
        var defaultFolders = new List<string>() { "Desktop", "Downloads", "Documents", "Music", "Pictures", "Videos" };

        var actionBlock = new ActionBlock<string>((defaultFolderPath) =>
        {
            var fullPath = Environment.ExpandEnvironmentVariables($"%USERPROFILE%\\{defaultFolderPath}");
            var backupPath = $"{path}\\{defaultFolderPath}";

            Directory.CreateDirectory(backupPath);

            foreach (var file in Directory.GetFiles(fullPath))
            {
                File.Copy(file, file.Replace(fullPath, backupPath));
            }

            foreach (var directory in Directory.GetDirectories(fullPath))
            {
                Directory.CreateDirectory(directory.Replace(fullPath, backupPath));

                try
                {
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        File.Copy(file, file.Replace(fullPath, backupPath));
                    }
                }
                catch (Exception)
                {
                }
            }

            progress.Report(1);
        }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 2, TaskScheduler = taskScheduler, });

        foreach (var folder in defaultFolders)
        {
            actionBlock.Post(folder);
        }

        actionBlock.Complete();

        return actionBlock.Completion;
    }

    public Task BackupGameData(string path, IProgress<int> progress)
    {
        var steamDefaultPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\SteamService")?.GetValue("installpath_default")?.ToString() ?? string.Empty;

        var actionBlock = new ActionBlock<(string, string)>(((string BackupFolder, string PathToCheck) input) =>
        {
            if (File.Exists(input.PathToCheck))
            {
                Directory.CreateDirectory(input.BackupFolder);

                File.Copy(input.PathToCheck, input.BackupFolder + $"\\{input.PathToCheck.Split('\\')[^1]}");
            }
            else if (Directory.Exists(input.PathToCheck))
            {
                Directory.CreateDirectory(input.BackupFolder);

                foreach (var file in Directory.GetFiles(input.PathToCheck))
                {
                    File.Copy(file, input.BackupFolder + $"\\{file.Split('\\')[^1]}");
                }
            }

             progress.Report(1);
        }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 5, TaskScheduler = taskScheduler, });

        foreach (var game in gameData)
        {
            var backupFolder = path + $"\\Video Games\\{Regex.Replace(game.Name, ":", string.Empty)}";

            foreach (var configPath in game.ConfigPaths)
            {
                var normalizedConfigPath = configPath.Contains("%STEAMPATH%") ? configPath.Replace("%STEAMPATH%", steamDefaultPath) : Environment.ExpandEnvironmentVariables(configPath);

                actionBlock.Post((backupFolder + "\\Config", normalizedConfigPath));
            }

            foreach (var saveGameDataPath in game.SaveGameDataPaths)
            {
                var normalizedSaveGameDataPath = saveGameDataPath.Contains("%STEAMPATH%") ? saveGameDataPath.Replace("%STEAMPATH%", steamDefaultPath) : Environment.ExpandEnvironmentVariables(saveGameDataPath);

                actionBlock.Post((backupFolder + "\\Config", saveGameDataPath));
            }
        }

        actionBlock.Complete();

        return actionBlock.Completion;
    }

    public void SaveProgamsAndUpdates(string path)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Programs:");
        programs.OrderBy(x => x).ToList().ForEach(x => stringBuilder.AppendLine(x));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Updates:");
        updates.OrderBy(x => x).ToList().ForEach(x => stringBuilder.AppendLine(x));

        File.WriteAllText(path + "\\Programs and updates.txt", stringBuilder.ToString());
    }

    private void GetProgamsAndUpdates()
    {
        var programs = new List<string>();
        var updates = new List<string>();
        var registryKeys = new string[] { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall" };

        foreach (string registryKey in registryKeys)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (var subkey = key.OpenSubKey(subkey_name))
                    {
                        var displayName = subkey.GetValue("DisplayName")?.ToString() ?? string.Empty;

                        if (string.IsNullOrWhiteSpace(displayName)) { continue; }

                        if (IsProgram(subkey))
                        {
                            programs.Add(displayName);
                        }
                        else
                        {
                            updates.Add(displayName);
                        }
                    }
                }
            }
        }   

        this.programs = programs.OrderBy(x => x).ToList();
        this.updates = updates.OrderBy(x => x).ToList();
    }

    private List<GameData> GetGameData()
    {
        var list = new List<GameData>();

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Emu.GameData.json"))
        {
            var reader = new StreamReader(stream);

            var array = JsonConvert.DeserializeObject<JArray>(reader.ReadToEnd());

            foreach (var token in array)
            {
                var gameData = new GameData()
                {
                    Name = token["Url"].ToString().Replace("https://www.pcgamingwiki.com/wiki/", string.Empty).Replace("_", " "),
                    ConfigPaths = token["Details"]["ConfigPath"]?.Values<string>().ToList() ?? new List<string>(),
                    SaveGameDataPaths = token["Details"]["SaveGameDataPath"]?.Values<string>().ToList() ?? new List<string>(),
                };

                if (!gameData.ConfigPaths.Any() && !gameData.SaveGameDataPaths.Any())
                {
                    continue;
                }

                list.Add(gameData);
            }
        }

        return list;
    }

    private bool IsProgram(RegistryKey subkey)
    {
        var hasDisplayName = !string.IsNullOrWhiteSpace(subkey.GetValue("DisplayName")?.ToString() ?? null);
        var hasDisplayIcon = !string.IsNullOrWhiteSpace(subkey.GetValue("DisplayIcon")?.ToString() ?? null);
        var hasBundleVersion = !string.IsNullOrWhiteSpace(subkey.GetValue("BundleVersion")?.ToString() ?? null);
        var hasUrlInfoAbout = !string.IsNullOrWhiteSpace(subkey.GetValue("URLInfoAbout")?.ToString() ?? null);

        return hasDisplayName && (hasDisplayIcon || hasUrlInfoAbout) && !hasBundleVersion;
    }
}
