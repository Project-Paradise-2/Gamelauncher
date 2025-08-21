using System;
using System.Windows.Input;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// A command implementation that allows binding actions to UI elements, such as buttons.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _executeFunc;

        /// <summary>
        /// Occurs when the ability of the command to execute changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execute and can-execute logic.
        /// </summary>
        /// <param name="execute">The action to execute when the command is triggered.</param>
        /// <param name="executeFunc">The function to determine if the command can execute (optional).</param>
        public RelayCommand(Action<object> execute, Func<object, bool> executeFunc = null)
        {
            _execute = execute;
            _executeFunc = executeFunc;
        }

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter to evaluate the command's execution eligibility.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object parameter) => _executeFunc == null || _executeFunc(parameter);

        /// <summary>
        /// Executes the command, triggering the action associated with it.
        /// </summary>
        /// <param name="parameter">The parameter to pass to the execute action.</param>
        public void Execute(object parameter) => _execute(parameter);
    }
}