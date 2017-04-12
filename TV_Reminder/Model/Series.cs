﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TV_Reminder.Model
{
    class Series
    {
        public string _description { get; set; }

        public string _title { get; set; }

        public int _season_number { get; set; }

        public int _episode_number { get; set; }

        public int _id { get; set; }

        public bool _watched { get; set; }

        public ImageSource _poster { get; set; }


        //Różne konstruktory

        public Series(string title, string description, int season_number, int episode_number, int id)
        {
            this._title = title;
            this._description = description;
            this._episode_number = episode_number;
            this._season_number = season_number;
            this._id = id;
        }

        public Series(string title, string description, int season_number, int episode_number, int id, bool watched)
        {
            this._title = title;
            this._description = description;
            this._episode_number = episode_number;
            this._season_number = season_number;
            this._id = id;
            this._watched = watched;
        }

        public Series(string title, string description, int season_number, int episode_number, int id, ImageSource poster)
        {
            this._title = title;
            this._description = description;
            this._episode_number = episode_number;
            this._season_number = season_number;
            this._id = id;
            this._poster = poster;
        }

        public Series(string title, string description, int season_number, int episode_number, int id, ImageSource poster, bool watched)
        {
            this._title = title;
            this._description = description;
            this._episode_number = episode_number;
            this._season_number = season_number;
            this._id = id;
            this._watched = watched;
            this._poster = poster;
        }
    }
}