using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Machine_Learning
{
    public partial class Menu : Form
    {
        public Settings settings;
        public static Settings minimums = new Settings(300, 300, 4, 4, 200, 0, 2.0, 1, 1.0, false, false, false, false);
        public static Settings maximums = new Settings(2000, 2000, 80, 80, 20000, 900, 20.0, 180, 16.0, true, true, true, true);

        public Menu()
        {
            InitializeComponent();
        }

        private int in_bounds(int num, int min, int max)
        {
            return Math.Max(min, Math.Min(max, num));
        }

        private double in_bounds(double num, double min, double max)
        {
            return Math.Max(min, Math.Min(max, num));
        }

        private void go_Click(object sender, EventArgs e)
        {
            bool significant = false;
            Settings old = Program.settings;
            Program.settings = new Settings();

            Program.settings.win_width = in_bounds(Convert.ToInt32(tbox_win_width.Text), minimums.win_width, maximums.win_width);
            Program.settings.win_height = in_bounds(Convert.ToInt32(tbox_win_height.Text), minimums.win_height, maximums.win_height);

            Program.settings.maze_width = in_bounds(Convert.ToInt32(tbox_maze_width.Text), minimums.maze_width, maximums.maze_width);
            Program.settings.maze_height = in_bounds(Convert.ToInt32(tbox_maze_height.Text), minimums.maze_height, maximums.maze_height);
            Program.settings.maze_time = in_bounds(Convert.ToInt32(tbox_maze_time.Text), minimums.maze_time, maximums.maze_time);

            Program.settings.bot_number = in_bounds(Convert.ToInt32(tbox_bot_num.Text), minimums.bot_number, maximums.bot_number);
            Program.settings.bot_radius = in_bounds(Convert.ToDouble(tbox_bot_radius.Text), minimums.bot_radius, maximums.bot_radius);
            Program.settings.bot_eyes = in_bounds(Convert.ToInt32(tbox_bot_eyes.Text), minimums.bot_eyes, maximums.bot_eyes);
            Program.settings.bot_speed = in_bounds(Convert.ToDouble(tbox_bot_speed.Text), minimums.bot_speed, maximums.bot_speed);

            Program.settings.learn_distance = check_learn_distance.Checked;

            Program.settings.think_distance = check_thinking_distance.Checked;
            Program.settings.think_path = check_thinking_path.Checked;
            Program.settings.think_velocity = check_thinking_velocity.Checked;

            significant |= old.bot_number != Program.settings.bot_number;
            significant |= old.bot_eyes != Program.settings.bot_eyes;
            significant |= old.learn_distance != Program.settings.learn_distance;
            significant |= old.think_distance != Program.settings.think_distance;
            significant |= old.think_path != Program.settings.think_path;
            significant |= old.think_velocity != Program.settings.think_velocity;

            Program.maze_form = new Maze_Form();

            if (Program.genalg == null || significant)
                Program.genalg = new GeneticAlg(Program.maze_form.ClientSize.Width - 1, Program.maze_form.ClientSize.Height - 1);

            Hide();
            Program.maze_form.Show();
        }
    }
}
