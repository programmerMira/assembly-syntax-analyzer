using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace OC_7
{
    public class ReadFile : IReadFile
    {
        #region Private Fields
        private Boolean openFile = false;
        private Boolean readFile = false;
        private Boolean checkFile = false;
        private Boolean correctFile = false;
        private String fileNotFoundMessage = "File doesn`t exist ";
        private String exeptionToSend="Mistake here!";
        private List<String[]> programData = new List<string[]>(); 
        #endregion

        #region Public Fields(get-set)
        public Boolean fileIsOpened
        {
            get
            {
                return openFile;
            }
            private set { }
        }
        public Boolean fileIsRead
        {
            get
            {
                return readFile;
            }
            private set { }
        }
        public Boolean fileIsChecked
        {
            get
            {
                return checkFile;
            }
            private set { }
        }
        public Boolean fileIsCorrect
        {
            get
            {
                return correctFile;
            }
            private set { }
        }
        #endregion

        #region Constructors
        public ReadFile() { }
        public ReadFile(String path) { }
        #endregion

        #region Private Methods
        /// <summary>
        /// The function checks if the used variable is assined
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkForExistingData(String dataName)
        {
            for(int i = 0; i < programData.Count; i++)
            {
                for(int j = 0; j < programData[i].Length; j++)
                {
                    if (programData[i][j] == dataName)
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// The function checks if the regester belongs to two byte regester type
        /// </summary>
        /// <param name="partCommand"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkTwoByteRegesters(String partCommand)
        {
            if (!partCommand.TrimEnd(',').Equals("AX") &&
                !partCommand.TrimEnd(',').Equals("BX") &&
                !partCommand.TrimEnd(',').Equals("CX") &&
                !partCommand.TrimEnd(',').Equals("DX") &&
                !partCommand.TrimEnd(',').Equals("CS"))
                return true;
            else return false;
        }
        /// <summary>
        /// The function checks if the regester belongs to one byte regester type
        /// </summary>
        /// <param name="partCommand"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkOneByteRegesters(String partCommand)
        {
            if (!partCommand.TrimEnd(',').Equals("AL") &&
                !partCommand.TrimEnd(',').Equals("AH") &&
                !partCommand.TrimEnd(',').Equals("BL") &&
                !partCommand.TrimEnd(',').Equals("BH") &&
                !partCommand.TrimEnd(',').Equals("CL") &&
                !partCommand.TrimEnd(',').Equals("CH") &&
                !partCommand.TrimEnd(',').Equals("DL") &&
                !partCommand.TrimEnd(',').Equals("DH"))
                return true;
            else return false;
        }
        /// <summary>
        /// The function checks the order of the command:
        /// - if it starts with the command word(add, sub or push)
        /// - if it has appropriate length
        /// - if it has a comma between the parameters
        /// - if all correct parameters are mentioned
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkCommandOrder(String command)
        {
            Regex regex = new Regex(@"[\d]");
            string[] command_parted = command.Split(' ');
            if (command_parted.Length < 2)
            {
                exeptionToSend = "Command is too short!";
                return false;
            }
            if (command_parted.Length==2 && command_parted[0] != "PUSH")
            {
                exeptionToSend = "The command requires two parameters!";
                return false;
            }
            if (command_parted[0] != "PUSH" && !command_parted[1].EndsWith(',') && !command_parted[2].StartsWith(','))
            {
                exeptionToSend = "The command requires two parameters seperated with dot!";
                return false;
            }
            if (checkTwoByteRegesters(command_parted[1]) && checkOneByteRegesters(command_parted[1]))
            {
                if (!checkForExistingData(command_parted[1].TrimEnd(',')))
                {
                    exeptionToSend = "First parameter after comand must be declared or must be a register!";
                    return false;
                }
                if (regex.IsMatch(command_parted[1]))
                {
                    exeptionToSend = "First parameter after comand must not be a number!";
                    return false;
                }
            }
            if (command_parted.Length == 3)
            {
                if (!regex.IsMatch(command_parted[2]) 
                    && (checkTwoByteRegesters(command_parted[1]) && checkOneByteRegesters(command_parted[2]))
                    || (checkTwoByteRegesters(command_parted[2]) && checkForExistingData(command_parted[1])))
                {
                    exeptionToSend = "Second parameter must be number or appropriate register!";
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// The function checks if the hex number is mentioned correctly
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkIfHex(String command)
        {
            if (command.EndsWith('H') &&
                !command.StartsWith('0') &&
                !command.StartsWith('1') &&
                !command.StartsWith('2') &&
                !command.StartsWith('3') &&
                !command.StartsWith('4') &&
                !command.StartsWith('5') &&
                !command.StartsWith('6') &&
                !command.StartsWith('7') &&
                !command.StartsWith('8') &&
                !command.StartsWith('9'))
                return false;
            else 
                return true;
        }
        /// <summary>
        /// The function checks if the binary number is mentioned correctly
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkIfBin(String command)
        {
            if (command.EndsWith('B') &&
                (command.Contains('2') ||
                command.Contains('3') ||
                command.Contains('4') ||
                command.Contains('5') ||
                command.Contains('6') ||
                command.Contains('7') ||
                command.Contains('8') ||
                command.Contains('9')))
                return false;
            else
                return true;
        }
        /// <summary>
        /// The function checks the order of the declaration command
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkCommandDeclarationOrder(String command)
        {
            string[] command_parted = command.Split(' ');
            programData.Add(command_parted);
            if (command_parted.Length < 3)
            {
                exeptionToSend = "The comand is too short for declaration!";
                return false;
            }
            if (command_parted[1].Equals("DB") && !command_parted[2].EndsWith('H') && 
                !command_parted[2].EndsWith('B') && int.Parse(command_parted[2]) > int.Parse("255"))
            {
                exeptionToSend = "DB contains number under 255";
                return false;
            }               
            if (!command_parted[1].Equals("DB") && !command_parted[1].Equals("DW"))
            {
                exeptionToSend = "Declaration must be only DB or DW";
                return false;
            }
            if (!checkIfHex(command_parted[2]))
            {
                exeptionToSend = "Hex number must start with number(0-9)";
                return false;
            }
            if (!checkIfBin(command_parted[2]))
            {
                exeptionToSend = "Binary number must contain 1s and 0s only!";
                return false;
            }
            return true;
        }
        /// <summary>
        /// The function seperates the declaration commands from programm commands and call the functions:
        /// checkCommandDeclarationOrder or checkCommandOrder
        /// </summary>
        /// <param name="command"></param>
        /// <returns>
        /// Boolean value
        /// </returns>
        private Boolean checkCommand(String command)
        {
            command = command.Replace(",", ", ");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            command = regex.Replace(command.ToUpper(), " ");

            command = command.Replace(" ,", ",");
            if (command.StartsWith("ADD") ||
                command.StartsWith("SUB") ||
                command.StartsWith("PUSH"))
            {
                if(checkCommandOrder(command))
                    return true;
            }
            else
            {
                if (checkCommandDeclarationOrder(command))
                    return true;
            }

            return false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The function reads data from txt or asm file and writes it to List<String>
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// List<String> fileData
        /// </returns>
        public List<String> openFileToRead(String path)
        {
            List<String> fileData = new List<string>();

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    openFile = true;
                    String tmp_line;
                    while ((tmp_line = sr.ReadLine()) != null)
                    {
                        if (tmp_line.Trim() != "")
                        {
                            fileData.Add(tmp_line.Trim());
                        }
                    }
                }
            }
            else
                throw new FileNotFoundException(fileNotFoundMessage,path);

            readFile = true;
            return fileData;
        }
        /// <summary>
        /// The function calls private function CheckCommand with a command as a parameter, 
        /// if the command is not correct, number of the string and description of the exception
        /// would be written in the dictionary and the function continues checking the commands
        /// from the following one.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// Dictionary<Int32, String> exceptions
        /// </returns>
        public Dictionary<Int32, String> checkDataForExs(List<String> data)
        {
            Dictionary<Int32,String> exeptionList = new Dictionary<Int32, String>();

            for(int i = 0; i<data.Count; i++)
            {
                if (!checkCommand(data[i]))
                {
                    exeptionList.Add(i, exeptionToSend);
                }
            }

            if (exeptionList.Count == 0)
                correctFile = true;
            checkFile = true;

            return exeptionList;
        }
        #endregion
    }
}
