using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WMPLib;
using Hook = KeyboardHooker.KeyboardHook;

namespace SpeechBot
{
    public partial class Form1 : Form
    {

        public Hook listener = new Hook();

        public SpeechSynthesizer synth = new SpeechSynthesizer();
        public SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

        public string beginListen, endListen, repeat;
        public string[] ActivationKeywords;

        public string[] categories;


        public List<string> steamGameList;
        public List<int> steamGameToken;


        public List<string> appList;
        public List<string> appToken;

        public string[] arguments;


        public List<string> questionList;
        public List<Action> questionToken;

        public List<string> commandList;
        public List<Action> commandToken;

        public List<string> scriptList;
        public List<dynamic> scriptToken;
        public Dictionary<string, string> codeDict = new Dictionary<string, string>();

        public List<string>[] categoryList;

        public Dictionary<string, int> steamGameDict = new Dictionary<string, int>();
        public Dictionary<string, string> appDict = new Dictionary<string, string>();
        public Dictionary<string, Action> questionDict = new Dictionary<string, Action>();
        public Dictionary<string, Action> commandDict = new Dictionary<string, Action>();

        public Dictionary<string, object> scriptDict = new Dictionary<string, object>();

        public int steamStartIndex, appStartIndex;

        public dynamic[] dictArray;


        public Grammar currentCommands;

        private bool l;
        public bool listening
        {
            get { return l; }
            set { Debug.WriteLine("setting to: " + value); l = value; }
        }

        public string saveLocation = Environment.ExpandEnvironmentVariables(@"%Public%\Documents\SpeechBot\Data\Save.dat");

        public Form1()
        {
            InitializeComponent();
        }

        public WindowsMediaPlayer player = new WindowsMediaPlayer();

        public string SoundsLocation = Environment.ExpandEnvironmentVariables(@"%Public%\Documents\SpeechBot\Sounds\");

        Form2 form2;

        SaveData sd;

        AudioPlaybackEngine ape1;
        AudioPlaybackEngine ape2;
        public void Form1_Load(object sender, EventArgs e)
        {
            InputBox.Instance = new InputBox();
            InputBox.Instance.StealthHide();
            InputBox.Instance.Visible = true;
            InputBox.Instance.Visible = false;
            InputBox.Instance.Init();

            this.MinimumSize = new Size(860, 375);

            form2 = new Form2(this);
            form2.Show();




            beginListen = "start"; endListen = "end"; repeat = "again";

            ActivationKeywords = new string[] { beginListen, endListen, repeat };

            categories = new string[] { "open steam game", "open app", "question", "command", "script" };

            steamGameList = new List<string> { "rainbow six siege", "counter strike" };
            steamGameToken = new List<int> { 359550, 730 };

            appList = new List<string> { "explorer", "notepad", "command prompt", "chrome", "chrome incognito", "brave", "brave incognito", "computer lock", "widget embed" };
            appToken = new List<string> { "explorer.exe", "notepad.exe", "cmd.exe", @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe --incognito", @"C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe", @"C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe --incognito", @"F:\Program Files\VS Project Files\ComputerLock\bin\Debug\ComputerLock.exe", @"F:\Program Files\VS Project Files\WidgetEmbed\bin\x64\Debug\WidgetEmbed.exe" };


            arguments = new string[] { "--incognito" };


            questionList = new List<string> { "enter a question here" };
            questionToken = new List<Action> { new Action(() => { }), new Action(() => { }) };

            commandList = new List<string> { "fix my display", "time check", "play me a sound" };
            commandToken = new List<Action> { new Action(() => { FixDisplay(); }), new Action(() => { TellTime(); }), new Action(() => { PlayASound(); }) };

            scriptList = new List<string>();
            scriptToken = new List<dynamic>();
            
            categoryList = new List<string>[] { steamGameList, appList, questionList, commandList, scriptList };
            dictArray = new dynamic[] { steamGameDict, appDict, questionDict, commandDict, scriptDict };


            steamStartIndex = steamGameList.Count;
            appStartIndex = appList.Count;


            this.CategoriesCoBox.DrawMode = DrawMode.OwnerDrawFixed;
            CategoriesCoBox.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < categories.Length; i++)
            {
                CategoriesCoBox.Items.Add(categories[i]);
            }

            string appDir = Environment.ExpandEnvironmentVariables(@"%Public%\Documents\SpeechBot");

            if (!Directory.Exists(appDir))
            {
                Directory.CreateDirectory(appDir);

                if (!Directory.Exists(appDir + @"\Sounds"))
                    Directory.CreateDirectory(appDir + @"\Sounds");

                if (!Directory.Exists(appDir + @"\Data"))
                    Directory.CreateDirectory(appDir + @"\Data");

            }
            
            if (!File.Exists(saveLocation))
            {
                File.Create(saveLocation);
            }
            else
            {
                sd = Extensions.Deserialize<SaveData>(saveLocation);
                for (int i = 0; i < sd.SteamGameList.Count; i++)
                {
                    steamGameList.Add(sd.SteamGameList[i]);
                    steamGameToken.Add(sd.SteamGameToken[i]);

                    form2.dataGridView1.Rows.Add(Category.SteamGame, sd.SteamGameList[i], sd.SteamGameToken[i], "■", "■");
                }

                for (int i = 0; i < sd.AppList.Count; i++)
                {
                    appList.Add(sd.AppList[i]);
                    appToken.Add(sd.AppToken[i]);

                    form2.dataGridView1.Rows.Add(Category.App, sd.AppList[i], sd.AppToken[i], "■", "■");
                }

                for (int i = 0; i < sd.ScriptList.Count; i++)
                {
                    scriptList.Add(sd.ScriptList[i]);
                    codeDict.Add(sd.ScriptList[i], sd.CodeToken[i]);

                    scriptDict.Add(sd.ScriptList[i], null);

                    form2.dataGridView1.Rows.Add(Category.Script, sd.ScriptList[i], sd.CodeToken[i], "■", "■");
                }
            }

            listener.Install();
            listener.KeyDown += KeyDown;
            listener.KeyUp += KeyUp;




            synth.SetOutputToDefaultAudioDevice();


            for (int i = 0; i < steamGameList.Count; i++)
            {
                steamGameDict.Add(steamGameList[i], steamGameToken[i]);
            }

            for (int i = 0; i < appList.Count; i++)
            {
                appDict.Add(appList[i], appToken[i]);
            }

            for (int i = 0; i < questionList.Count; i++)
            {
                questionDict.Add(questionList[i], questionToken[i]);
            }

            for (int i = 0; i < commandList.Count; i++)
            {
                commandDict.Add(commandList[i], commandToken[i]);
            }

            DictationGrammar dg = new DictationGrammar("grammar:dictation#pronunciation");
            dg.Name = "Random";


            LoadCommands();

            rec.LoadGrammar(dg);
            rec.MaxAlternates = 1;

            rec.SpeechRecognized += rec_SpeechRecognized;
            rec.SetInputToDefaultAudioDevice();

        }

        public void LoadCommands()
        {
            Choices gb = new Choices(new string[] { beginListen, endListen, repeat });
            for (int i = 0; i < categories.Length; i++)
            {
                for (int j = 0; j < categoryList[i].Count; j++)
                {
                    gb.Add(categories[i] + " " + categoryList[i][j]);

                }
            }

            currentCommands = new Grammar(gb);

            currentCommands.Name = "commands";

            rec.LoadGrammar(currentCommands);

            preparedHelp = false;
        }

        public void ReloadCommands()
        {
            rec.UnloadGrammar(currentCommands);

            LoadCommands();
        }


        public static Random r = new Random();
        public void FixDisplay()
        {
            Process.Start(@"F:\program Files\vs Project Files\displaySettingsMenu\bin\Debug\DisplaySettingsMenu.exe", "--FixDisplay");
        }

        public void TellTime()
        {
            synth.SpeakAsync("The time is currently " + DateTime.Now.ToString("dddd, MMMM dd, hh:mm tt"));
        }

        public void PlaySoundsThroughMic(params string[][] sounds)
        {
            if (ape1 == null && ape2 == null)
            {
                ape1 = new AudioPlaybackEngine();
                ape2 = new AudioPlaybackEngine();

                InitAPE(ape1, 1, 1);
                InitAPE(ape2, 0, 2);
            }
            else
            {
                ape1.StopAllSounds();
                ape2.StopAllSounds();
            }

            string sound = sounds[r.Next(0, sounds.Length)].GetRandomValue<string>();
            if (!File.Exists(sound))
            {
                sound = SoundsLocation + sound;
            }

            ape1.PlaySound(sound);
            ape2.PlaySound(sound);

        }

        public void PlaySoundThroughMic(string sound)
        {
            if (ape1 == null && ape2 == null)
            {
                ape1 = new AudioPlaybackEngine();
                ape2 = new AudioPlaybackEngine();

                InitAPE(ape1, 1, 1);
                InitAPE(ape2, 0, 2);
            }
            else
            {
                ape1.StopAllSounds();
                ape2.StopAllSounds();
            }

            ape1.PlaySound(sound);
            ape2.PlaySound(sound);
        }

        private void InitAPE(AudioPlaybackEngine ape, int PlaybackDevice, int PlaybackDeviceNumber)
        {
            try
            {
                ape.Init(PlaybackDevice);
            }
            catch (NAudio.MmException ex)
            {
                SystemSounds.Beep.Play();

                string msg = ex.ToString();

                if (msg.Contains("AlreadyAllocated calling waveOutOpen"))
                {
                    msg = "Failed to open device. Already in exclusive use by another application? \n\n" + msg;
                }

                MessageBox.Show("Playback " + PlaybackDeviceNumber + msg);
            }
        }

        public bool VoiceKeyDown;
        public bool RepeatKeyDown;
        public bool TryingToRecognize;
        Hook.VKeys RequiredKey = Hook.VKeys.RSHIFT;
        Hook.VKeys RepeatKey = Hook.VKeys.PRIOR;
        private void KeyDown(Hook.VKeys key)
        {

            if (key == RequiredKey && !VoiceKeyDown)
            {
                rec.RecognizeAsync(RecognizeMode.Multiple);
                TryingToRecognize = true;
                VoiceKeyDown = true;
            }

            if (key == RepeatKey && !RepeatKeyDown)
            {
                if (data == null)
                    return;

                RunCommand(data);
                return;
            }
        }
        private void KeyUp(Hook.VKeys key)
        {
            if (key == Hook.VKeys.APPS)
            {
                Debug.WriteLine("KeyIsDown: " + VoiceKeyDown);
                Debug.WriteLine("TryingToRecognize: " + TryingToRecognize);
            }

            if (key == RequiredKey)
            {
                rec.RecognizeAsyncStop();
                VoiceKeyDown = false;
            }

            if (key == RepeatKey)
            {
                RepeatKeyDown = false;
            }
        }

        void Beep(int freq, int ms)
        {
            Thread t = new Thread(() =>
            {
                Console.Beep(freq, ms);
            });
            t.Start();
        }

        void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            TryingToRecognize = false;

            if (e.Result.Grammar.Name == "Random")
                return;



            Debug.WriteLine("Confidence: " + e.Result.Confidence);

            Debug.WriteLine("Recognized Text: " + e.Result.Text);
            string text = e.Result.Text.ToLower();

            if (text == beginListen)
            {
                if (listening)
                    return;

                listening = true;
                Beep(1750, 250);
                return;
            }
            if (text == endListen)
            {
                if (!listening)
                    return;

                listening = false;
                Beep(1000, 250);
                return;
            }

            if (listening)
            {
                CheckCommand(text);
            }
        }

        public enum Category
        {
            SteamGame,
            App,
            Question,
            Command,
            Script
        }

        Query QueryData = new Query();
        Query GetData(string text)
        {
            for (int i = 0; i < categories.Length; i++)
                if (text.Length >= categories[i].Length)
                    if (text.Substring(0, categories[i].Length) == categories[i])
                        QueryData.Category = (Category)i;

            QueryData.Data = text.Substring(categories[(int)QueryData.Category].Length + 1);

            return QueryData;
        }

        Query data;
        void CheckCommand(string text)
        {
            if (text == repeat)
            {
                if (data == null)
                    return;

                RunCommand(data);
                return;
            }

            data = GetData(text);

            RunCommand(data);
        }

        private bool HelpShown;
        private bool preparedHelp;
        private string helpInfo;
        private void HelpButton_Click(object sender, EventArgs e)
        {
            if (!preparedHelp)
                helpInfo = PrepareHelp();

            if (HelpShown)
                return;

            HelpShown = true;

            Task.Run(() =>
            {
                MessageBox.Show(helpInfo, "Commands", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HelpShown = false;
            });
        }

        private string PrepareHelp()
        {
            preparedHelp = true;
            string test = "";

            test += "Activation Keywords:\n\n";

            for (int i = 0; i < ActivationKeywords.Length; i++)
                test += ActivationKeywords[i] + "\n";

            test += "\n\n\n";


            for (int i = 0; i < categories.Length; i++)
            {
                test += categories[i] + ":\n\n";
                for (int j = 0; j < categoryList[i].Count; j++)
                {
                    test += categoryList[i][j] + "\n";
                }

                if (!(categories.Length - 1 == i))
                    test += "\n\n\n";
            }
            return test;
        }


        string defaultScriptText =
@"<DLLs>
</DLLs>

<CodeSpace>
</CodeSpace>";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CategoriesCoBox.SelectedIndex == 2 || CategoriesCoBox.SelectedIndex == 3)
            {
                CategoriesCoBox.SelectedIndex = -1;
                return;
            }

            if (!(CategoriesCoBox.SelectedIndex == 4))
            {
                CommandDataTBox.Text = "";
                label1.Hide();
            }
            else
            {
                CommandDataTBox.Text = defaultScriptText;
                label1.Show();
            }

        }

        Font FontEnabled = new Font("Aerial", 10, FontStyle.Regular);
        Font FontDisabled = new Font("Aerial", 10, FontStyle.Strikeout);
        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == 2 || e.Index == 3)
            {
                e.Graphics.DrawString(CategoriesCoBox.Items[e.Index].ToString(), FontDisabled, Brushes.Red, e.Bounds);
            }
            else
            {
                if (e.Index == -1)
                    return;

                e.DrawBackground();
                e.Graphics.DrawString(CategoriesCoBox.Items[e.Index].ToString(), FontEnabled, Brushes.Black, e.Bounds);
                e.DrawFocusRectangle();
            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            HelpButton.Focus();
        }

        int steamIDParse;
        string DataText;
        string CommandName;

        List<string> DLLs = new List<string>();
        string[] defaultDLLs = new string[] { "mscorlib.dll", "System.Windows.Forms.dll", "System.dll", "System.Drawing.dll", "System.Core.dll", "Microsoft.VisualBasic.dll", "Microsoft.CSharp.dll" };
        string[] UserDLLs;

        string beginningCode = @"
    using System.Windows.Forms;
    using System.Drawing;
    public class Script 
    {
        public void Function(dynamic form)
        {
";
        string endingCode =
        @"}
    }";

        public string Code;
        CSharpCodeProvider csc = new CSharpCodeProvider();
        CompilerParameters parameters;

        string[] ExistsTitles = new string[] { "Steam Game", "Application", "Question", "Command", "Script" };
        private void CreateCommand_Click(object sender, EventArgs e)
        {
            if (CategoriesCoBox.SelectedIndex < 0 || CategoriesCoBox.SelectedIndex >= CategoriesCoBox.Items.Count)
            {
                DisplayError("Invalid ComboBox Index");
                return;
            }

            CommandName = CommandNameTBox.Text.ToLower().Trim();

            for (int i = 0; i < dictArray.Length; i++)
            {
                if (CategoriesCoBox.SelectedIndex == i && dictArray[i].ContainsKey(CommandName))
                {
                    DisplayError(ExistsTitles[i] + " - Name Already Exists");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(CommandName))
            {
                DisplayError("Command Name was [Empty]");
                return;
            }

            DataText = CommandDataTBox.Text;
            switch (CategoriesCoBox.SelectedIndex)
            {

                case 0:
                    DataText = DataText.Replace(" ", "").Replace(Environment.NewLine, "");

                    if (int.TryParse(DataText, out steamIDParse))
                    {
                        steamGameList.Add(CommandName);
                        steamGameDict.Add(CommandName, steamIDParse);

                        string[] keys = steamGameDict.Keys.ToArray();
                        int[] values = steamGameDict.Values.ToArray();

                    }
                    else
                    {
                        DisplayError("Text is not an [Integer]");
                        return;
                    }

                    break;

                case 1:
                    if (File.Exists(DataText))
                    {
                        appList.Add(CommandName);
                        appDict.Add(CommandName, DataText);
                    }
                    else
                    {
                        DisplayError("Text does not point to a [File Location]");
                        return;
                    }
                    
                    break;

                case 2: //not implemented
                    break;

                case 3: //not implemented
                    break;

                case 4:
                    string randomName = CommandName.Replace(" ", "_") + r.Next(10000, 100000);

                    CompilerResults results = CompileCode(DataText, "public class " + randomName);

                    if (results == null)
                        return;

                    if (!results.Errors.HasErrors)
                    {
                        codeDict.Add(CommandName, DataText);

                        scriptList.Add(CommandName);

                        scriptDict.Add(CommandName, MakeScriptObject(results, randomName));
                    }
                    else
                    {
                        var errors = string.Join(Environment.NewLine,
                            results.Errors.Cast<CompilerError>().Select(x => x.ErrorText));
                        MessageBox.Show(errors);
                        return;
                    }

                    break;

            }
            if (ReloadCreateChBox.Checked)
                ReloadCommands();

            form2.dataGridView1.Rows.Add((Category)CategoriesCoBox.SelectedIndex, CommandName, DataText, "■", "■");
        }

        /// <summary>
        /// Compiles Code based on Text Input. - (Make sure to null check as it will return null on error)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public CompilerResults CompileCode(string Data, string replaceText)
        {
            if (!(Data.Contains("<DLLs>") && Data.Contains("</DLLs>") && Data.Contains("<CodeSpace>") && Data.Contains("</CodeSpace>")))
            {
                DisplayError("Improper Script Formatting");
                if (MessageBox.Show("Would you like to reset the scripting textbox to the default text?", "Reset Scripting Textbox", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    CommandDataTBox.Text = defaultScriptText;

                return null;
            }

            Code = getBetween(Data, "<CodeSpace>", "</CodeSpace");

            List<string> blacklistResult = ReturnIfContains(Code.ToLower(), "file.delete", "directory.delete", "del *", "erase *", "rd *", "rmdir *", "rename *", "replace *");

            if (blacklistResult.Count > 0)
            {
                string blacklistString = "";
                for (int i = 0; i < blacklistResult.Count; i++)
                {
                    blacklistString += "\r\n\"" + blacklistResult[i] + "\"";
                }

                DisplayError("Code contains blacklisted keywords:" + blacklistString);
                return null;
            }

            UserDLLs = getBetween(Data, "<DLLs>", "</DLLs>").SplitToLines().ToArray();

            DLLs.Clear();
            DLLs.AddRange(defaultDLLs);
            DLLs.AddRange(UserDLLs);
            parameters = new CompilerParameters(DLLs.ToArray());

            return csc.CompileAssemblyFromSource(parameters, beginningCode.Replace("public class Script", replaceText) + Code + endingCode);
        }

        public object MakeScriptObject(CompilerResults results, string TypeAsString = "Script")
        {
            Type t = results.CompiledAssembly.GetType(TypeAsString);
            return Activator.CreateInstance(t);
        }

        public bool CheckIfContains(string input, params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (input.Contains(strings[i]))
                    return true;
            }

            return false;
        }

        List<string> ContainedWords = new List<string>();
        public List<string> ReturnIfContains(string input, params string[] strings)
        {
            ContainedWords.Clear();
            for (int i = 0; i < strings.Length; i++)
            {
                if (input.Contains(strings[i]))
                {
                    ContainedWords.Add(strings[i]);
                }
            }

            return ContainedWords;
        }

        private void Reload_Button(object sender, EventArgs e)
        {
            ReloadCommands();
        }

        public void DisplayError(string msg, string title = "Command Creation - Error")
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void RunCommand(Query data)
        {
            Debug.WriteLine("Category: " + data.Category);

            switch (data.Category)
            {
                case Category.SteamGame:
                    synth.SpeakAsync("Starting Game, " + data.Data);
                    Process.Start("steam://rungameid/" + (int)Parse(data.Category, data.Data));
                    break;

                case Category.App:
                    synth.SpeakAsync("Starting Application, " + data.Data);

                    string AppLocation = appDict[data.Data];
                    string Arguments = "";

                    for (int i = 0; i < arguments.Length; i++)
                        if (AppLocation.Contains(arguments[i]))
                        {
                            Debug.WriteLine("CONTAINS");
                            Arguments += arguments[i] + " ";
                            AppLocation = AppLocation.Replace(arguments[i], "");
                        }

                    Arguments = Arguments.Trim();
                    AppLocation = AppLocation.Trim();
                    Debug.WriteLine(AppLocation);
                    Debug.WriteLine(Arguments);


                    if (Arguments.Length > 0)
                        Process.Start((string)AppLocation, Arguments);
                    else
                        Process.Start((string)Parse(data.Category, data.Data));
                    break;

                case Category.Question:
                    Parse(data.Category, data.Data);
                    break;

                case Category.Command:
                    Parse(data.Category, data.Data);
                    break;

                case Category.Script:
                    Parse(data.Category, data.Data);
                    break;
            }
        }

        private void ShowChart_Click(object sender, EventArgs e)
        {
            if (!form2.Visible)
                form2.Show();

            form2.BringToFront();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            synth.Dispose();
            rec.RecognizeAsyncStop();
            rec.Dispose();
            listener.KeyDown -= KeyDown;
            listener.KeyUp -= KeyUp;
            listener.Uninstall();
            player.close();

            ape1?.Dispose();
            ape2?.Dispose();

            form2.Save(false);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            form2.Save();
        }

        public object Parse(Category cat, string data)
        {
            switch (cat)
            {
                case Category.SteamGame:
                    return steamGameDict[data];

                case Category.App:
                    return appDict[data];

                case Category.Question:
                    questionDict[data]();
                    return null;

                case Category.Command:
                    commandDict[data]();
                    return null;

                case Category.Script:
                    if (scriptDict[data] == null)
                    {
                        scriptDict.Remove(data);

                        string randomName = data.Replace(" ", "_") + r.Next(10000, 100000);
                        

                        scriptDict.Add(data, (object)MakeScriptObject(CompileCode(codeDict[data], "public class " + randomName), randomName));
                    }
                    ((dynamic)scriptDict[data]).Function(this);
                    return null;
            }

            return null;
        }


        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }

    public static class Extensions
    {
        public static T GetRandomValue<T>(this T[] array)
        {
            return array[Form1.r.Next(0, array.Length)];
        }

        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static void Serialize<T>(this T value, string outputFile)
        {

            if (value == null)
                MessageBox.Show("The object submitted for serialization was null", "Serialization - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(T));

                using (TextWriter tw = new StreamWriter(outputFile))
                {
                    s.Serialize(tw, value);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static T Deserialize<T>(string fileLocation)
        {
            T deserializedObject = default(T);

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (TextReader tr = new StreamReader(fileLocation))
                {
                    deserializedObject = (T)xmlSerializer.Deserialize(tr);
                    tr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured", ex);
            }
            
            return deserializedObject;
        }
    }

    public class Query
    {
        public Form1.Category Category;
        public string Data;
    }

}
