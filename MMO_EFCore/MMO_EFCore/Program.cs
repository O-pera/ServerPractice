using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace MMO_EFCore {
    public class Program {
        //초기화는 시간이 많이 걸린다.
        #region Menu Display

        const string Index_0 = "Exit";
        const string Index_1 = "ForceReset";
        const string Index_2 = "ReadAll";
        const string Index_3 = "Eager";
        const string Index_4 = "Explicit";
        const string Index_5 = "Select";
        const string Index_6 = "";
        const string Index_7 = "";
        const string Index_8 = "";


        #endregion

        private static bool loop = true;
        static void Main(string[] args) {
            DbCommands.InitializeDB(forceReset: false);

            while(loop) {
                #region Main Menu
                Console.WriteLine($"Input Command");
                if(Index_0 != string.Empty) Console.WriteLine(String.Format("[0]: {0}", Index_0));
                if(Index_1 != string.Empty) Console.WriteLine(String.Format("[1]: {0}", Index_1));
                if(Index_2 != string.Empty) Console.WriteLine(String.Format("[2]: {0}", Index_2));
                if(Index_3 != string.Empty) Console.WriteLine(String.Format("[3]: {0}", Index_3));
                if(Index_4 != string.Empty) Console.WriteLine(String.Format("[4]: {0}", Index_4));
                if(Index_5 != string.Empty) Console.WriteLine(String.Format("[5]: {0}", Index_5));
                if(Index_6 != string.Empty) Console.WriteLine(String.Format("[6]: {0}", Index_6));
                if(Index_7 != string.Empty) Console.WriteLine(String.Format("[7]: {0}", Index_7));
                if(Index_8 != string.Empty) Console.WriteLine(String.Format("[8]: {0}", Index_8));
                Console.Write(">>> ");
                int command = int.Parse(Console.ReadLine());
                #endregion

                switch(command) {
                    case 0:  loop = false;                                  break;
                    case 1:  DbCommands.InitializeDB(forceReset: true);     break;
                    case 2:  DbCommands.ReadAll();                          break;
                    case 3:  DbCommands.Eager();                            break;
                    case 4:  DbCommands.Explicit();                         break;
                    case 5:  DbCommands.Select();                           break;
                }

                if(loop != false) Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
