using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    class WordList
    {
        //private string word;
        //private int score;
        //private string wordList;
        private string [] lines;

        public WordList(){

        }

        public void readFile(string fileToRead){
            try
            {
                lines = new string[0];
                lines = File.ReadAllLines(fileToRead);
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.");
            }
            catch (System.IO.PathTooLongException)
            {
                Console.WriteLine("The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Console.WriteLine("The specified path is invalid (for example, it is on an unmapped drive).");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("An I/O error occurred while opening the file.");
            }
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine("path specified a file that is read-only.-or- This operation is not supported on the current platform.-or- path specified a directory.-or- The caller does not have the required permission.");
            }
            catch (System.NotSupportedException)
            {
                Console.WriteLine("path is in an invalid format.");
            }
            catch(System.Security.SecurityException){
                Console.WriteLine("The caller does not have the required permission.");
            }
        }

        public void constructTrie(Trie trie) {
            foreach (string line in lines)
            {
                trie.Add(line);
            }
        }

        //public void Addall(Trie trie) { 
        //    while((line = textReader.ReadLine()) != null){
        //        trie.Add(line);
        //    }
        //}

        //public void Close(){
        //    textReader.Close();
        //}
    }
}
