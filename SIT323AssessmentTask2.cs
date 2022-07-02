using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ConfigurationDataLibrary;
using System.Threading;

namespace SIT323AssessmentTask2
{
    public partial class SIT323AssessmentTask2Form : Form
    {

        private ErrorForm ErrorForm { get; set; }
        public static List<string> CompleteErrorList = new List<string>();
        private TaskAllocations aTaskAllocation = new TaskAllocations();
        private Configuration aConfiguration = new Configuration();
        string fileValiditys = "";
        public static List<string> endpointAdresses = new List<string>();


        //constants
        private const string diffEnergyError = "Allocation ID: {0} has a different energy value {1:0.00} to Allocation ID: 1 with energy {2:0.00}";
        private const string startTanFile = "START PROCESSING TAN FILE: ";
        private const string endTanFile = "END PROCESSING TAN FILE: ";
        private const string tanFileValid = "TAN file is valid\n\n";
        private const string tanFileInvalid = "TAN file is invalid\n\n";
        private const string fileNameMissing = "File Name Missing";
        private const string csvFileValid = "Configuration file is valid\n\n";
        private const string csvFileInvalid = "Configuration file is invalid\n\n";
        private const string startCsvFile = "START PROCESSING CONFIG FILE: ";
        private const string endCsvFile = "END PROCESSING CONFIG FILE: ";
        private const string allocationDisplayString = "Allocation ID: {0}\n";
        private const string startAllocations = "START PROCESSING ALLOCATIONS:";
        private const string endAllocations = "END PROCESSING ALLOCATIONS: ";
        private const string allocationTimeEnergyDisplay = "Allocation ID: {0}, Time: {1:0.00}, Energy: {2:0.00}\n";

        public SIT323AssessmentTask2Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void OpenTANFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTANFile();

        }

        private void OpenTANFile()
        {
            //Clear any residual errors
            CompleteErrorList.Clear();
            label1.Text = "";
            fileValiditys = "";

            DialogResult result;
            result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ProcessTAN();
            }
        }

        private void ProcessTAN()
        {

            //Process TAN file
            CompleteErrorList.Add(startTanFile + openFileDialog1.SafeFileName);
            TaskAllocations.TryParse(openFileDialog1.FileName, out aTaskAllocation);
            if (aTaskAllocation.AllocationErrorList != null) UpdateErrors(aTaskAllocation.AllocationErrorList);
            CompleteErrorList.Add(endTanFile + openFileDialog1.SafeFileName);

            if (aTaskAllocation.isValid) fileValiditys += tanFileValid;
            else fileValiditys += tanFileInvalid;

            //Get directory of TAN file and add config path
            string directoryPath = Path.GetDirectoryName(openFileDialog1.FileName);
            string configFullPath = directoryPath + "\\" + aTaskAllocation.ConfigPath;

            if (aTaskAllocation.ConfigPath != null)
            {
                ProcessConfig(configFullPath);
            }

            if (aTaskAllocation.isValid == true && aConfiguration.isValid == true)
            {
                allocationsToolStripMenuItem.Enabled = true;
            }
            else
            {
                allocationsToolStripMenuItem.Enabled = false;
            }

            //Update text
            label1.Text = "";
            label1.Text += fileValiditys;
            foreach (Allocation al in aTaskAllocation.SetOfAllocations)
            {
                label1.Text += string.Format(allocationDisplayString, al.ID);
                label1.Text += al.MatrixToString();
            }
        }

        private void ProcessConfig(string filePath)
        {
            //Process CONFIG file

            string fileName = Path.GetFileName(filePath);
            CompleteErrorList.Add(startCsvFile + fileName);

            Configuration.TryParse(filePath, out aConfiguration);
            if (aConfiguration.ConfigurationErrorList != null) UpdateErrors(aConfiguration.ConfigurationErrorList);

            if (aConfiguration.isValid) fileValiditys += csvFileValid;
            else fileValiditys += csvFileInvalid;

            CompleteErrorList.Add(endCsvFile + fileName);
        }

        private void ErrorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorForm errorForm = new ErrorForm();
            errorForm.ShowDialog();
        }

        private void UpdateErrors(List<string> errors)
        {
            if (errors.Count > 0)
            {
                int errorCounter = 1;
                foreach (string str in errors)
                {
                    StringBuilder strBuild = new StringBuilder();
                    strBuild.Append("Error ");
                    strBuild.Append(errorCounter);
                    strBuild.Append(": ");
                    strBuild.Append(str);
                    CompleteErrorList.Add(strBuild.ToString());
                    errorCounter++;
                }
            }
        }


        private void allocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label1.Text += fileValiditys;
            double energyMarker = 0;


            //Calculate allocations
            CompleteErrorList.Add(startAllocations);
            foreach (Allocation al in aTaskAllocation.SetOfAllocations)
            {


                List<string> errors = new List<string>();

                //get time

                double time = al.CalculateTime(aConfiguration, out errors);
                UpdateErrors(errors);
                string timeString = time.ToString("0.00");

                //get energy
                double energy = al.CalculateEnergy(aConfiguration, out errors);
                UpdateErrors(errors);

                string energyString = energy.ToString("0.00");

                if (energyMarker == 0) energyMarker = energy;
                if (energy != energyMarker) errors.Add(string.Format(diffEnergyError, al.ID, al.AllocationEnergy, energyMarker));
                UpdateErrors(errors);

                //update text
                label1.Text += string.Format(allocationTimeEnergyDisplay, al.ID, timeString, energyString);
                label1.Text += al.MatrixToString();

            }

            CompleteErrorList.Add(endAllocations);
            allocationsToolStripMenuItem.Enabled = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            decimal maxRuntime = numericUpDown1.Value * 1000;

            //Clean GUI and Errors
            CompleteErrorList.Clear();
            label1.Text = "";
            fileValiditys = "";
            allocationsToolStripMenuItem.Enabled = false;

            string source = comboBox1.Text;
            if (source != null)
            {
                //Download file stored on internet
                string destination = Path.GetFileName(source);
                WebClient webclient = new WebClient();
                webclient.DownloadFile(source, destination);

                //Initialise Configuration Data
                string fullConfigPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + destination;
                ProcessConfig(fullConfigPath);
                ConfigurationData ConfData = new ConfigurationData(aConfiguration)
                {
                    AlgorithmMaxRuntime = maxRuntime
                };

                //Setup Local WCF Services
                LocalALG1WebService.ServiceClient LocalALG1 = new LocalALG1WebService.ServiceClient();
                LocalALG1.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);

                LocalALG2WebService.ServiceClient LocalALG2 = new LocalALG2WebService.ServiceClient();
                LocalALG2.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);

                LocalALG3WebService.ServiceClient LocalALG3 = new LocalALG3WebService.ServiceClient();
                LocalALG3.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);

                //Setup AWS WCF Services
                AwsALG1WebService.ServiceClient AwsALG1 = new AwsALG1WebService.ServiceClient();
                AwsALG1.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);

                AwsALG2WebService.ServiceClient AwsALG2 = new AwsALG2WebService.ServiceClient();
                AwsALG2.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);

                AwsALG3WebService.ServiceClient AwsALG3 = new AwsALG3WebService.ServiceClient();
                AwsALG3.InnerChannel.OperationTimeout = new TimeSpan(0, 5, 0);


                //Loading Bar setup
                int interval = 5 + ((Convert.ToInt32(numericUpDown1.Value) - 1) * 10);
                while (progressBar1.Value < 100)
                {
                    Thread.Sleep(interval);
                    progressBar1.Increment(1);
                }


                //Receive results from Web Services
                List<string[]> returnedStrings = new List<string[]>
                {
                    //await LocalALG1.GetAllocationsAsync(ConfData),
                    //await LocalALG2.GetAllocationsAsync(ConfData),
                    //await LocalALG3.GetAllocationsAsync(ConfData),
                    await AwsALG1.GetAllocationsAsync(ConfData),
                    await AwsALG2.GetAllocationsAsync(ConfData),
                    await AwsALG3.GetAllocationsAsync(ConfData)
                };
                
                //Close service clients
                LocalALG1.Close();
                LocalALG2.Close();
                LocalALG3.Close();
                AwsALG1.Close();
                AwsALG2.Close();
                AwsALG3.Close();

                //Create Allocations from returned strings
                List<Allocation> returnedAllocations = new List<Allocation>();
                endpointAdresses.Clear();

                foreach (string[] stringArray in returnedStrings)
                {
                    List<string> stringList = stringArray.ToList();
                    endpointAdresses.Add(stringList[0]);
                    stringList.RemoveAt(0);

                    foreach (string s in stringList)
                    {
                        List<string> matrixList = s.Split('\n').ToList();
                        Allocation allocation = new Allocation(matrixList);
                        returnedAllocations.Add(allocation);
                    }
                }

                //Display endpoint address's in popup
                EndpointForm endpointForm = new EndpointForm();
                endpointForm.Show();

                //Determine best set of allocations
                double energy;
                double bestEnergy = double.MaxValue;
                List<Allocation> bestAllocations = new List<Allocation>();
                int allocationIDCounter = 1;

                foreach (Allocation al in returnedAllocations)
                {
                    if (al.ValidateAllocation() == false)
                    {
                        returnedAllocations.Remove(al);
                    }
                    al.CalculateTime(aConfiguration);
                    energy = al.CalculateEnergy(aConfiguration);

                    if (energy < bestEnergy)
                    {
                        //Reset Allocation ID's
                        allocationIDCounter = 1;
                        al.ID = allocationIDCounter;
                        allocationIDCounter++;

                        bestAllocations.Clear();
                        bestAllocations.Add(al);
                        bestEnergy = energy;
                    }
                    else if (energy == bestEnergy)
                    {
                        if (!bestAllocations.Contains(al))
                        {
                            al.ID = allocationIDCounter;
                            allocationIDCounter++;
                            bestAllocations.Add(al);
                        }
                    }
                }


                //Update GUI
                label1.Text += fileValiditys;
                int idCounter = 1;

                //Show best allocations on GUI
                foreach (Allocation al in bestAllocations)
                {
                    label1.Text += al.ToString() + "\n\n";
                    idCounter++;
                }
            }


        }


    }
}
