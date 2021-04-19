namespace TinyLittleMvvm {
    /// <summary>
    /// Specifies the icon that is displayed by a message box. Used as an
    /// argument of the <see cref="IWindowManager.ShowMessageBox"/> method.
    /// </summary>
    public enum MessageBoxImage {
        /// <summary>
        /// The message box contains no symbols.
        /// </summary>
        None = System.Windows.MessageBoxImage.None,

        /// <summary>
        /// The message box contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Error = System.Windows.MessageBoxImage.Error,

        /// <summary>
        /// The message box contains a symbol consisting of a white X in a circle with a red background.
        /// </summary>
        Hand = System.Windows.MessageBoxImage.Hand,

        /// <summary>
        /// The message box contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Stop = System.Windows.MessageBoxImage.Stop,

        /// <summary>
        /// The message box contains a symbol consisting of a question mark in a circle. The question
        /// mark message icon is no longer recommended because it does not clearly represent a specific
        /// type of message and because the phrasing of a message as a question could apply to any
        /// message type. In addition, users can confuse the question mark symbol with a help information
        /// symbol. Therefore, do not use this question mark symbol in your message boxes. The system
        /// continues to support its inclusion only for backward compatibility.
        /// </summary>
        Question = System.Windows.MessageBoxImage.Question,

        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Exclamation = System.Windows.MessageBoxImage.Exclamation,

        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Warning = System.Windows.MessageBoxImage.Warning,

        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Asterisk = System.Windows.MessageBoxImage.Asterisk,

        /// <summary>
        /// The message box contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Information = System.Windows.MessageBoxImage.Information
    }
}