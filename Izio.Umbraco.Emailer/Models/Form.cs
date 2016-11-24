using System;
using Izio.Umbraco.Emailer.Interfaces;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Izio.Umbraco.Emailer.Models
{
    [TableName("izioEmailerForms")]
    public class Form : IDatabaseEntity
    {
        #region private properties

        private string _name;
        private Guid? _reference;
        private string _destinationAddress;
        private int _submissionLimit;
        private string _confirmationMessage;
        private string _templateSubject;
        private string _templateBody;
        private bool? _responderEnabled;
        private string _responderAddress;
        private string _responderSubject;
        private string _responderBody;

        #endregion

        #region public properties

        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != null) IsDirty = true;
                _name = value;
            }
        }

        public Guid Reference
        {
            get
            {
                if (_reference == null)
                {
                    _reference = Guid.NewGuid();
                }

                return _reference.Value;
            }
            set
            {
                if (_reference != null) IsDirty = true;
                _reference = value;
            }
        }

        public string DestinationAddress
        {
            get { return _destinationAddress; }
            set
            {
                if (_destinationAddress != null) IsDirty = true;
                _destinationAddress = value;
            }
        }

        public int SubmissionLimit
        {
            get { return _submissionLimit; }
            set
            {
                if (_submissionLimit != 0) IsDirty = true;
                _submissionLimit = value;
            }
        }

        public string ConfirmationMessage
        {
            get { return _confirmationMessage; }
            set
            {
                if (_confirmationMessage != null) IsDirty = true;
                _confirmationMessage = value;
            }
        }

        public string TemplateSubject
        {
            get { return _templateSubject; }
            set
            {
                if (_templateSubject != null) IsDirty = true;
                _templateSubject = value;
            }
        }

        public string TemplateBody
        {
            get { return _templateBody; }
            set
            {
                if (_templateBody != null) IsDirty = true;
                _templateBody = value;
            }
        }

        public bool ResponderEnabled
        {
            get { return _responderEnabled.HasValue  && _responderEnabled.Value; }
            set
            {
                if (_responderEnabled != null) IsDirty = true;
                _responderEnabled = value;
            }
        }

        public string ResponderAddress
        {
            get { return _responderAddress; }
            set
            {
                if (_responderAddress != null) IsDirty = true;
                _responderAddress = value;
            }
        }

        public string ResponderSubject
        {
            get { return _responderSubject; }
            set
            {
                if (_responderSubject != null) IsDirty = true;
                _responderSubject = value;
            }
        }

        public string ResponderBody
        {
            get { return _responderBody; }
            set
            {
                if (_responderBody != null) IsDirty = true;
                _responderBody = value;
            }
        }

        #endregion

        #region IDatabaseEntity

        [Ignore]
        public bool IsNew => Id == 0;

        [Ignore]
        public bool IsDirty { get; set; }

        #endregion
    }
}
