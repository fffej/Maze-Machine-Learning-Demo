using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.Neural
{
    public class NNet
    {
        private float[,] weights;
        private float[,] layers;
        private float[,] inputs_outputs;

        private int[] net_size;
        private int largest_layer;

        private static Random rand = new Random();

        public NNet(int[] network_params)
        {
            this.net_size = network_params;
            build();
        }

        public NNet(int num_inputs, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, num_outputs };
            build();
        }

        public NNet(int num_inputs, int layer_1, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, layer_1, num_outputs };
            build();
        }

        public NNet(int num_inputs, int layer_1, int layer_2, int num_outputs)
        {
            this.net_size = new int[] { num_inputs, layer_1, layer_2, num_outputs };
            build();
        }

        public float get_weight(int i, int j)
        {
            return weights[i, j];
        }

        public float generate_weight()
        {
            return (float)(rand.NextDouble() * 2.0 - 1.0);
        }

        public void build()
        {
            largest_layer = net_size.Max();

            weights = new float[net_size.Length, largest_layer * largest_layer];
            layers = new float[2, largest_layer];
            inputs_outputs = new float[2, largest_layer];

            for (int i = 0; i < net_size.Length; ++i)
                for (int j = 0; j < largest_layer * largest_layer; ++j)
                    weights[i, j] = generate_weight();
        }

        /// <summary>
        /// Breeds two parent brains creating this as the new child.
        /// </summary>
        /// <param name="parent_a">A parent brain.</param>
        /// <param name="parent_b">A parent brain.</param>
        /// <param name="mutation_rate">The rate of weights that are randomized instead of inherited.</param>
        public void breed(NNet parent_a, NNet parent_b, double mutation_rate)
        {
            for (int i = 0; i < net_size.Length; ++i)
            {
                for (int j = 0; j < largest_layer * largest_layer; ++j)
                {
                    if (rand.NextDouble() <= mutation_rate)
                    {
                        weights[i, j] = generate_weight();
                    }
                    else if (rand.NextDouble() >= 0.5)
                    {
                        weights[i, j] = parent_a.get_weight(i, j);
                    }
                    else
                    {
                        weights[i, j] = parent_b.get_weight(i, j);
                    }
                }
            }
        }

        public float get_input(int index)
        {
            return inputs_outputs[0, index];
        }

        public float get_output(int index)
        {
            return inputs_outputs[1, index];
        }

        public void set_input(int index, float input)
        {
            inputs_outputs[0, index] = input;
        }

        public void set_inputs(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; ++i)
            {
                inputs_outputs[0, i] = inputs[i];
            }
        }

        public float sigmoid(float a)
        {
            // This is a Log Sigmoid.
            return (float)(1.0 / (1.0 + Math.Exp(-a * 4.0)));
        }

        /// <summary>
        /// Use the two working layers to calculate the outputs of the network from given inputs.
        /// The working layer is switched after each network layer to conserve memory, and improve performance.
        /// </summary>
        public void think()
        {
            for (int n = 0; n < net_size[0]; ++n)
                layers[0, n] = inputs_outputs[0, n];

            for (int layer = 1; layer < net_size.Length; ++layer)
            {
                for (int neuron = 0; neuron < net_size[layer]; ++neuron)
                {
                    layers[layer % 2, neuron] = 0;

                    for (int synapse = 0; synapse < net_size[layer - 1]; ++synapse)
                        layers[layer % 2, neuron] += layers[1 - (layer % 2), synapse] * weights[layer - 1, (neuron + 1) * synapse];

                    layers[layer % 2, neuron] = sigmoid(layers[layer % 2, neuron]);
                }
            }

            for (int n = 0; n < net_size[net_size.Length - 1]; ++n)
                inputs_outputs[1, n] = layers[(net_size.Length - 1) % 2, n];
        }
    }
}
