using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using System.Runtime.CompilerServices;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Identification;
using Microsoft.Win32;
using System.IO;

namespace VoiceIdentification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedFile = "";
        private SpeakerIdentificationServiceClient _serviceClient = new SpeakerIdentificationServiceClient("1cb9ba6056ee4ba1be230ac93bc1358c");
        private WaveIn _waveIn;
        private WaveFileWriter _fileWriter;
        SortedList<Guid, string> enrollVoiceList = new SortedList<Guid, string>();
        string userName = "";

        public MainWindow()
        {
            InitializeComponent();
            InitializeRecorder();
        }

        /// <summary>
        /// Initialize NAudio recorder instance
        /// </summary>
        private void InitializeRecorder()
        {
            _waveIn = new WaveIn();
            _waveIn.DeviceNumber = 0;
            int sampleRate = 16000; // 16 kHz
            int channels = 1; // mono
            _waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            _waveIn.DataAvailable += waveIn_DataAvailable;
            _waveIn.RecordingStopped += waveSource_RecordingStopped;
        }

        /// <summary>
        /// A method that's called whenever there's a chunk of audio is recorded
        /// </summary>
        /// <param name="sender">The sender object responsible for the event</param>
        /// <param name="e">The arguments of the event object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (_fileWriter == null)
            {
                Console.WriteLine("Error");
            }
            _fileWriter.Write(e.Buffer, 0, e.BytesRecorded);
        }

        /// <summary>
        /// A listener called when the recording stops
        /// </summary>
        /// <param name="sender">Sender object responsible for event</param>
        /// <param name="e">A set of arguments sent to the listener</param>
        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            _fileWriter.Dispose();
            _fileWriter = null;

            //Dispose recorder object
            _waveIn.Dispose();
            InitializeRecorder();

        }

        /// <summary>
        /// Adds a speaker profile to the speakers list
        /// </summary>
        /// <param name="speaker">The speaker profile to add</param>
        public void AddSpeaker(Profile speaker)
        {
            Console.WriteLine("Adding profile...");
            //Console.WriteLine("Id is : {0}", speaker.ToString());
            Console.WriteLine("Another Id is : {0}", Guid.Parse(speaker.ProfileId.ToString()));
            //enrollVoiceList.Add(Guid.Parse(speaker.ToString()),userName);
            enrollVoiceList.Add(Guid.Parse(speaker.ProfileId.ToString()), userName);
            Console.WriteLine("Done..");
        }



        /// <summary>
        /// Retrieves all the speakers asynchronously and adds them to the list
        /// </summary>
        /// <returns>Task to track the status of the asynchronous task.</returns>
        public async Task UpdateAllSpeakersAsync()
        {
            SpeakerIdentificationServiceClient _serviceClient = new SpeakerIdentificationServiceClient("1cb9ba6056ee4ba1be230ac93bc1358c");
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            try
            {
                //window.Log("Retrieving All Profiles...");
                Title = String.Format("Retrieving All Profiles...");
                Profile[] allProfiles = await _serviceClient.GetProfilesAsync();
                //window.Log("All Profiles Retrieved.");
                Title = String.Format("All Profiles Retrieved.");
                enrollVoiceList.Clear();
                foreach (Profile profile in allProfiles)
                    AddSpeaker(profile);
                //_speakersLoaded = true;
            }
            catch (GetProfileException ex)
            {
                //window.Log("Error Retrieving Profiles: " + ex.Message);
                Title = String.Format("Error Retrieving Profiles: " + ex.Message);
            }
            catch (Exception ex)
            {
                //window.Log("Error: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void recordBtn_Click(object sender, RoutedEventArgs e)
        {
            recordBtn.IsEnabled = false;
            identifyBtn.IsEnabled = true;
            try
            {
                if (WaveIn.DeviceCount == 0)
                {
                    throw new Exception("Cannot detect microphone.");
                }

                string getDirectory = Directory.GetCurrentDirectory();
                _selectedFile = getDirectory + "\\Sample2.wav";
                _fileWriter = new NAudio.Wave.WaveFileWriter(_selectedFile, _waveIn.WaveFormat);
                _waveIn.StartRecording();
                
                Title = String.Format("Recording...");
            }
            catch (Exception ge)
            {
                Console.WriteLine("Error: " + ge.Message);
            }
        }

        //private void loadFileBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    identifyBtn.IsEnabled = true;
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "WAV Files(*.wav)|*.wav";
        //    bool? result = openFileDialog.ShowDialog();

        //    if (!(bool)result)
        //    {
        //        Title = String.Format("No File Selected.");
        //        return;
        //    }
        //    Title = String.Format("File Selected: " + openFileDialog.FileName);
        //    _selectedFile = openFileDialog.FileName;
        //}

        
        private async void identifyBtn_Click(object sender, RoutedEventArgs e)
        {
            _identificationResultStckPnl.Visibility = Visibility.Hidden;
            //First Stop Recording...
            recordBtn.IsEnabled = true;
            identifyBtn.IsEnabled = false;
            if (_waveIn != null)
            {
                _waveIn.StopRecording();
            }

            TimeSpan timeBetweenSaveAndIdentify = TimeSpan.FromSeconds(5.0);
            await Task.Delay(timeBetweenSaveAndIdentify);
            //await UpdateAllSpeakersAsync();

            //Identify Voice
            //List<Guid> selectedItems = new List<Guid>(3);
            //selectedItems.Add(Guid.Parse("f7d5a9d2-9663-4504-b53c-ee0c2c975104")); //f7d5a9d2-9663-4504-b53c-ee0c2c975104
            //selectedItems.Add(Guid.Parse("acec28d0-cfd5-4bc4-8840-03bb523a43f7"));
            //selectedItems.Add(Guid.Parse("7ce81071-ef9d-46cf-9d87-02d465b1a972"));
            //selectedItems.Add(Guid.Parse("f7d5a9d2-9663-4504-b53c-ee0c2c975104"));
            //selectedItems.Add(Guid.Parse("0501e357-e56d-46b0-87ad-957ef8744d9c"));
            //selectedItems.Add(Guid.Parse("aeb46767-24b7-4d9d-a9ad-dd7dc965b0bb"));
            //selectedItems.Add(Guid.Parse("26c11b8d-6de2-4c5e-971c-8c4dc774d5e1"));
            //selectedItems.Add(Guid.Parse("f7d5a9d2-9663-4504-b53c-ee0c2c975104"));
            //selectedItems.Add(Guid.Parse("8f85b2c6-688a-4d44-a8c4-370be910b8bf"));

            //Guid[] selectedIds = new Guid[1];
            //int i = 0;
            //foreach (KeyValuePair<Guid, string> kv in enrollVoiceList)
            //{
            //    selectedIds[i] = kv.Key;
            //    i++;
            //}
            //selectedIds[0] = selectedItems[0];

            //for (int j = 0; j < 1; j++)
            //{
            //    selectedIds[i] = selectedItems[i];

            //}

            List<Guid> list = new List<Guid>();
            Profile[] allProfiles = await _serviceClient.GetProfilesAsync();
            int itemsCount = 0;
            foreach (Profile profile in allProfiles)
            {
                list.Add(profile.ProfileId);
                itemsCount++;
            }
            Guid[] selectedIds = new Guid[1];
            for(int i = 0; i < 1; i++)
            {
                selectedIds[i] = list[i];
            }

            try
            {
                if (_selectedFile == "")
                    throw new Exception("No File Selected.");

                Title = String.Format("Identifying File...");
                OperationLocation processPollingLocation;
                using (Stream audioStream = File.OpenRead(_selectedFile))
                {
                    _selectedFile = "";
                    Console.WriteLine("Audio File is : {0}", audioStream);
                    processPollingLocation = await _serviceClient.IdentifyAsync(audioStream, selectedIds, true);
                }

                IdentificationOperation identificationResponse = null;
                int numOfRetries = 10;
                TimeSpan timeBetweenRetries = TimeSpan.FromSeconds(5.0);
                while (numOfRetries > 0)
                {
                    await Task.Delay(timeBetweenRetries);
                    identificationResponse = await _serviceClient.CheckIdentificationStatusAsync(processPollingLocation);
                    Console.WriteLine("Response is : {0}", identificationResponse);
                    if (identificationResponse.Status == Status.Succeeded)
                    {
                        break;
                    }
                    else if (identificationResponse.Status == Status.Failed)
                    {
                        throw new IdentificationException(identificationResponse.Message);
                    }
                    numOfRetries--;
                }
                if (numOfRetries <= 0)
                {
                    throw new IdentificationException("Identification operation timeout.");
                }

                Title = String.Format("Identification Done.");

                if (identificationResponse.ProcessingResult.IdentifiedProfileId.ToString() == "aeb46767-24b7-4d9d-a9ad-dd7dc965b0bb")
                {
                    _identificationResultTxtBlk.Text = "Toshif"; //aeb46767 - 24b7 - 4d9d - a9ad - dd7dc965b0bb //f7d5a9d2-9663-4504-b53c-ee0c2c975104
                }

                else if (identificationResponse.ProcessingResult.IdentifiedProfileId.ToString() == "7ce81071-ef9d-46cf-9d87-02d465b1a972")
                {
                    _identificationResultTxtBlk.Text = "Aakash";
                }
                else if (identificationResponse.ProcessingResult.IdentifiedProfileId.ToString() == "acec28d0-cfd5-4bc4-8840-03bb523a43f7")
                {
                    _identificationResultTxtBlk.Text = "Mohammad Toshif Khan"; //0501e357 - e56d - 46b0 - 87ad - 957ef8744d9c
                }
                else if (identificationResponse.ProcessingResult.IdentifiedProfileId.ToString() == "f7d5a9d2-9663-4504-b53c-ee0c2c975104")
                {
                    _identificationResultTxtBlk.Text = "Nandini"; //f7d5a9d2 - 9663 - 4504 - b53c - ee0c2c975104
                }
                else
                {
                    _identificationResultTxtBlk.Text = identificationResponse.ProcessingResult.IdentifiedProfileId.ToString();
                }
                _identificationConfidenceTxtBlk.Text = identificationResponse.ProcessingResult.Confidence.ToString();
                _identificationResultStckPnl.Visibility = Visibility.Visible;
            }
            catch (IdentificationException ex)
            {
                GC.Collect();
                Title = String.Format("Speaker Identification Error: " + ex.Message);
                Console.WriteLine("Speaker Identification Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                GC.Collect();
                Title = String.Format("Error: " + ex.Message);
            }

            GC.Collect();
        }
    }
        
}
