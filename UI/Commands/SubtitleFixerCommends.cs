﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static SubtitleFixer.SubtitleFixerViewModel; 

namespace SubtitleFixer
{
    /// <summary>
    /// this class is for fix subs, the disrence from other command is only in CanExecute method
    /// </summary>
    public class FixMovieSubCommends : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public FixMovieSubCommends(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public FixMovieSubCommends(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        private bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool CanExecute(object parameter)
        {
            if (IsList(parameter))
            {
                // the parameter 1 is the bool of selection status
                // the parameter 0 is the string of File Path
                // only if status is for folder and File path not empty
                List<object> list = (List<object>)parameter;
                return this.canExecute != null && this.canExecute(parameter) &&
                       ((PATH_TYPE)list[1]) == PATH_TYPE.File && list[0] != null && list[0].ToString() != "";
            }

            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }

    /// <summary>
    /// this class is for fix subs, the disrence from other command is only in CanExecute method
    /// </summary>
    public class FixSeriesSubsCommends : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public FixSeriesSubsCommends(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public FixSeriesSubsCommends(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        private bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool CanExecute(object parameter)
        {
            if (IsList(parameter))
            {
                // the parameter 1 is the bool of selection status
                // the parameter 0 is the string of Folder Path
                // only if status is for folder and folder path not empty
                List<object> list = (List<object>)parameter;
                return this.canExecute != null && this.canExecute(parameter) &&
                       ((PATH_TYPE)list[1]) == PATH_TYPE.Folder && list[0] != null && list[0].ToString() != "";
            }

            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
    /// <summary>
    /// this class is for fix encode, the disrence from other command is only in CanExecute method
    /// </summary>
    public class FixEncodeCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public FixEncodeCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public FixEncodeCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        private bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool CanExecute(object parameter)
        {
            if (IsList(parameter))
            {
                // the parameter 2 is the bool of selection status
                // the parameter 0 is the string of Folder Path
                // the parameter 1 is the string of File Path
                // if is folder status check that folder is not empty else check the file path 
                List<object> list = (List<object>)parameter;
                return this.canExecute != null && this.canExecute(parameter) &&
                       ((PATH_TYPE)list[2]) == PATH_TYPE.Folder ? list[0]!= null && list[0].ToString() != "" : list[1] != null &&  list[1].ToString() != "";
            }

            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }

    /// <summary>
    /// simple command without any spacile conditions or methods
    /// </summary>
    public class SimpleCommends : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public SimpleCommends(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public SimpleCommends(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
