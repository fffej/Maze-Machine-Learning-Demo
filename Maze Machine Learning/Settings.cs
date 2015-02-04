using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning
{
    public struct Settings
    {
        public int win_width;
        public int win_height;

        public int maze_width;
        public int maze_height;
        public int maze_time;

        public int bot_number;
        public double bot_radius;
        public int bot_eyes;
        public double bot_speed;

        public bool learn_distance;

        public bool think_velocity;
        public bool think_distance;
        public bool think_path;

        public Settings(int window_width, int window_height,
                        int maze_width, int maze_height, int maze_cutoff_time,
                        int number_of_bots, double bot_radius,
                        int number_of_eyes, double bot_speed,
                        bool learn_distance, bool think_distance, bool think_velocity, bool think_path)
        {
            this.win_width = window_width;
            this.win_height = window_height;

            this.maze_width = maze_width;
            this.maze_height = maze_height;
            this.maze_time = maze_cutoff_time;

            this.bot_number = number_of_bots;
            this.bot_radius = bot_radius;
            this.bot_eyes = number_of_eyes;
            this.bot_speed = bot_speed;

            this.learn_distance = learn_distance;

            this.think_distance = think_distance;
            this.think_path = think_path;
            this.think_velocity = think_velocity;
        }
    }
}
