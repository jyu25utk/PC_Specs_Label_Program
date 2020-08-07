﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PC_Specs_Label_Program
{
    class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;

        public CommandHandler(Action act, bool canExec)
        {
            _action = act;
            _canExecute = canExec;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
