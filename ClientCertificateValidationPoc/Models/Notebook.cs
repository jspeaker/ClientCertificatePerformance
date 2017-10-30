using System.Collections.Generic;

namespace ClientCertificateValidationPoc.Models
{
    public interface INotebook
    {
        void NewNote(string note);

        IEnumerable<string> AllPages();
    }

    public class Notebook : INotebook
    {
        private readonly List<string> _messages = new List<string>();

        public void NewNote(string note)
        {
            _messages.Add(note);
        }

        public IEnumerable<string> AllPages()
        {
            return _messages;
        }
    }
}