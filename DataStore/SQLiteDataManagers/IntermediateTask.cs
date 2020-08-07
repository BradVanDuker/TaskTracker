﻿using DataStore.DataManagers;
using System;
using System.Collections.Generic;
using TaskTracker;


namespace DataStore.SQLiteDataManagers
{
    public class IntermediateTask
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { this._Id = value; }
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToUserId { get; set; }
        public int SourceUserId { get; set; }
        public string DateCreated { get; set; }
#nullable enable

        private string? _DateAssigned;
        public string? DateAssigned
        {
            get { return _DateAssigned; }
            set { _DateAssigned = ConvertBlanksToNull(value); }
        }

        private string? _DateCompleted;
        public string? DateCompleted
        {
            get { return _DateCompleted; }
            set { _DateCompleted = ConvertBlanksToNull(value); }
        }

        private string? _Notes;
        public string? Notes
        {
            get { return _Notes; }
            set { _Notes = ConvertBlanksToNull(value); }
        }

        private string? ConvertBlanksToNull(string? value)
        {
            return value != "" ? value : null;
        }
#nullable disable

        public List<object> ToList()
        {
            var myProps = this.GetType().GetProperties();
            var values = new List<object>();
            foreach (var prop in myProps)
            {
                values.Add(prop.GetValue(this));
            }
            return values;
        }

        public Task ToTask(UserManager userManager)
        {
#nullable enable
            static DateTime? processNullableDate(string? stringRep)
            {
                if (stringRep == null || stringRep == "")
                {
                    return null;
                }
                else
                {
                    return DateTime.Parse(stringRep);
                }
            }
#nullable disable
            var dateCreated = processNullableDate(this.DateCreated);
            var dateAssigned = processNullableDate(this.DateAssigned);
            var dateCompleted = processNullableDate(this.DateCompleted);

            var task = new Task(
                title: this.Title,
                description: this.Description,
                assignedTo: userManager.Get(this.AssignedToUserId),
                source: userManager.Get(this.SourceUserId),
                id: this.Id,
                dateCreated: dateCreated,
                dateAssigned: dateAssigned,
                dateCompleted: dateCompleted
                );
            return task;
        }
    }
}
