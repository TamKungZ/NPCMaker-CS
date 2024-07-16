using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace NPCMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
         private void ClearTextOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void RestorePlaceholder(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = textBox.Name switch
                {
                    "nameMod" => "Mod Name",
                    "authorEntry" => "Author",
                    "versionEntry" => "Version",
                    "descriptionEntry" => "Description",
                    "uniqueIdEntry" => "Unique ID",
                    "nexsusIdEntry" => "Nexus ID",
                    "nameNpc" => "NPC Name",
                    "positionLocationEntry" => "Position Location",
                    "positionXEntry" => "Position X",
                    "positionYEntry" => "Position Y",
                    "directionEntry" => "Direction",
                    "favorMessageEntry" => "Favor Message",
                    "likeMessageEntry" => "Like Message",
                    "normalMessageEntry" => "Normal Message",
                    "dislikeMessageEntry" => "Dislike Message",
                    "hateMessageEntry" => "Hate Message",
                    "favorIdEntry" => "Favor ID",
                    "likeIdEntry" => "Like ID",
                    "normalIdEntry" => "Normal ID",
                    "dislikeIdEntry" => "Dislike ID",
                    "hateIdEntry" => "Hate ID",
                    "otherEntry" => "Other",
                    "npcAgeVar" => "NPC Age",
                    "firstVar" => "First",
                    "secondVar" => "Second",
                    "thirdVar" => "Third",
                    "genderVar" => "Gender",
                    "datetingVar" => "Dateting",
                    "sessionBirthdayVar" => "Session Birthday",
                    "birthdayNumberVar" => "Birthday Number",
                    "fv" => "Format Version",
                    "portraitEntry" => "Portrait File Path",
                    "spriteEntry" => "Sprite File Path",
                    _ => textBox.Text
                };
            }
        }


        private void ExportJsonManifest(object sender, RoutedEventArgs e)
        {
            string modName = nameMod.Text;
            string author = authorEntry.Text;
            string version = versionEntry.Text;
            string description = descriptionEntry.Text;
            string uniqueId = uniqueIdEntry.Text;
            string nexsusId = nexsusIdEntry.Text;

            if (!string.IsNullOrEmpty(modName) && !string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(version) && 
                !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(uniqueId) && !string.IsNullOrEmpty(nexsusId))
            {
                var data = new
                {
                    Name = modName,
                    Author = author,
                    Version = version,
                    Description = description,
                    UniqueID = uniqueId,
                    UpdateKeys = new[] { $"Nexus:{nexsusId}" },
                    ContentPackFor = new { UniqueID = "Pathoschild.ContentPatcher" }
                };

                string folderName = modName;
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                string filePath = Path.Combine(folderName, "manifest.json");
                File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));

                MessageBox.Show($"Manifest data has been exported to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please fill in all fields", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExportJsonContent(object sender, RoutedEventArgs e)
        {
            string npcName = nameNpc.Text;
            string positionLocation = positionLocationEntry.Text;
            string positionX = positionXEntry.Text;
            string positionY = positionYEntry.Text;
            string direction = directionEntry.Text;
            string favorMessage = favorMessageEntry.Text;
            string likeMessage = likeMessageEntry.Text;
            string normalMessage = normalMessageEntry.Text;
            string dislikeMessage = dislikeMessageEntry.Text;
            string hateMessage = hateMessageEntry.Text;
            string favorId = favorIdEntry.Text;
            string likeId = likeIdEntry.Text;
            string normalId = normalIdEntry.Text;
            string dislikeId = dislikeIdEntry.Text;
            string hateId = hateIdEntry.Text;
            string other = otherEntry.Text;

            string npcAge = npcAgeVar.Text;
            string first = firstVar.Text;
            string second = secondVar.Text;
            string third = thirdVar.Text;
            string gender = genderVar.Text;
            string dateting = datetingVar.Text;
            string sessionBirthday = sessionBirthdayVar.Text;
            string birthdayNumber = birthdayNumberVar.Text;

            string formatVer = fv.Text;

            var data = new
            {
                Format = formatVer,
                Changes = new[]
                {
                    new { Action = "EditData", Target = "Data/NPCDispositions", Entries = new Dictionary<string, string> { { npcName, $"{npcAge}/{first}/{second}/{third}/{gender}/{dateting}/{other}/{sessionBirthday} {birthdayNumber}//{positionLocation} {positionX} {positionY} {direction}/{npcName}" } } },
                    new { Action = "EditData", Target = "Data/NPCGiftTastes", Entries = new Dictionary<string, string> { { npcName, $"{favorMessage}/{favorId}/{likeMessage}/{likeId}/{normalMessage}/{normalId}/{dislikeMessage}/{dislikeId}/{hateMessage}/{hateId}" } } },
                    new { LogName = "Load Portraits", Action = "Load", Target = $"Portraits/{npcName}", FromFile = $"img/{npcName}_portraits.png" },
                    new { LogName = "Load Sprites", Action = "Load", Target = $"Characters/{npcName}", FromFile = $"img/{npcName}_sprites.png" },
                    new { LogName = "Load Schedule", Action = "Load", Target = $"Characters/schedules/{npcName}", FromFile = $"json/{npcName}_schedule.json" },
                    new { LogName = "Load Dialogue", Action = "Load", Target = $"Characters/Dialogue/{npcName}", FromFile = $"json/{npcName}_dialogue.json" }
                }
            };

            string modName = nameMod.Text;

            string folderName = modName;
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string filePath = Path.Combine(folderName, "content.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));

            MessageBox.Show($"Content data has been exported to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TextureSet(object sender, RoutedEventArgs e)
        {
            string portraitPath = portraitEntry.Text;
            string spritePath = spriteEntry.Text;
            string npcName = nameNpc.Text;
            string modName = nameMod.Text;

            if (!string.IsNullOrEmpty(portraitPath) && !string.IsNullOrEmpty(spritePath) && !string.IsNullOrEmpty(npcName) && !string.IsNullOrEmpty(modName))
            {
                string folderName = Path.Combine(modName, "img");
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                try
                {
                    string portraitDest = Path.Combine(folderName, $"{npcName}_portraits.png");
                    string spriteDest = Path.Combine(folderName, $"{npcName}_sprites.png");

                    File.Copy(portraitPath, portraitDest, true);
                    File.Copy(spritePath, spriteDest, true);

                    MessageBox.Show("Textures have been exported successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to copy files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields and select files", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BrowseFile(TextBox entry)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files (*.png)|*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                entry.Text = openFileDialog.FileName;
            }
        }

        private void ExportScheduleJson(object sender, RoutedEventArgs e)
        {
            string npcName = nameNpc.Text;
            string modName = nameMod.Text;

            var scheduleData = new Dictionary<string, string>
            {
                { "Mon", GetDaySchedule(monScheduleEntries) },
                { "Tue", GetDaySchedule(tueScheduleEntries) },
                { "Wed", GetDaySchedule(wedScheduleEntries) },
                { "Thu", GetDaySchedule(thuScheduleEntries) },
                { "Fri", GetDaySchedule(friScheduleEntries) },
                { "Sat", GetDaySchedule(satScheduleEntries) },
                { "Sun", GetDaySchedule(sunScheduleEntries) }
            };

            string folderName = Path.Combine(modName, "json");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string filePath = Path.Combine(folderName, $"{npcName}_schedule.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(scheduleData, Formatting.Indented));

            MessageBox.Show($"Schedule data has been exported to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddScheduleEntry(List<(ComboBox timeVar, TextBox scheduleVar)> dayScheduleEntries, StackPanel panel)
        {
            ComboBox timeVar = new ComboBox
            {
                ItemsSource = times
            };
            TextBox scheduleVar = new TextBox();

            StackPanel entryPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            entryPanel.Children.Add(timeVar);
            entryPanel.Children.Add(scheduleVar);

            panel.Children.Add(entryPanel);
            dayScheduleEntries.Add((timeVar, scheduleVar));
        }

        private string GetDaySchedule(List<(ComboBox timeVar, TextBox scheduleVar)> entries)
        {
            var scheduleList = new List<string>();
            foreach (var (timeVar, scheduleVar) in entries)
            {
                if (!string.IsNullOrEmpty(timeVar.Text) && !string.IsNullOrEmpty(scheduleVar.Text))
                {
                    scheduleList.Add($"{timeVar.Text} {scheduleVar.Text}");
                }
            }
            return string.Join("/", scheduleList);
        }

        private void ExportDialogueJson(object sender, RoutedEventArgs e)
        {
            string npcName = nameNpc.Text;
            string modName = nameMod.Text;

            var dialogueData = new Dictionary<string, string>();
            foreach (var day in dialogueEntries.Keys)
            {
                var entries = dialogueEntries[day];
                for (int i = 0; i < entries.Count; i++)
                {
                    dialogueData[$"{day}{i}"] = entries[i].Text;
                }
            }

            string folderName = Path.Combine(modName, "json");
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string filePath = Path.Combine(folderName, $"{npcName}_dialogue.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(dialogueData, Formatting.Indented));

            MessageBox.Show($"Dialogue data has been exported to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddDialogueEntry(string day, StackPanel panel)
        {
            TextBox dialogueVar = new TextBox();
            panel.Children.Add(dialogueVar);
            dialogueEntries[day].Add(dialogueVar);
        }

        private readonly List<string> times = new List<string> { "6:00am", "7:00am", "8:00am", "9:00am", "10:00am", "11:00am", "12:00pm", "1:00pm", "2:00pm", "3:00pm", "4:00pm", "5:00pm", "6:00pm", "7:00pm", "8:00pm", "9:00pm", "10:00pm" };

        private readonly Dictionary<string, List<TextBox>> dialogueEntries = new Dictionary<string, List<TextBox>>
        {
            { "Mon", new List<TextBox>() },
            { "Tue", new List<TextBox>() },
            { "Wed", new List<TextBox>() },
            { "Thu", new List<TextBox>() },
            { "Fri", new List<TextBox>() },
            { "Sat", new List<TextBox>() },
            { "Sun", new List<TextBox>() }
        };

        private readonly List<(ComboBox, TextBox)> monScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> tueScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> wedScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> thuScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> friScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> satScheduleEntries = new List<(ComboBox, TextBox)>();
        private readonly List<(ComboBox, TextBox)> sunScheduleEntries = new List<(ComboBox, TextBox)>();
    }
}
