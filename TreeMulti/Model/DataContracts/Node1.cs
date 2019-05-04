﻿using System;

namespace TreeMulti.Model.DataContracts
{
    [Serializable]
    public class Node1 : Model.Node
    {
        private string _comment2;

        public Node1()
        {

        }
        public Node1(string name, string comment, string comment2) :base(name,comment)
        {
            Comment2 = comment2;
        }

        public string Comment2
        {
            get => _comment2;
            set
            {
                if (value == _comment2)
                {
                    return;
                }
                _comment2 = value;
                OnPropertyChanged();
            }
        }
    }
}
