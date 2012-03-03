﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.ML.Data;
using Encog.Util.KMeans;
using System.Collections;

namespace Encog.ML.HMM.Train.KMeans
{
    /// <summary>
    /// Clusters used for the KMeans HMM training algorithm.
    /// </summary>
    public class Clusters
    {
        /// <summary>
        /// Provide quick access to the clusters.
        /// </summary>
        private IDictionary<IMLDataPair, int> clustersHash;

        /// <summary>
        /// A list of all of the clusters.
        /// </summary>
        private IList<ICollection<IMLDataPair>> clusters;

        /// <summary>
        /// Construct the clusters objects. 
        /// </summary>
        /// <param name="k">The number of clusters to have.</param>
        /// <param name="observations">The observations.</param>
        public Clusters(int k, IMLDataSet observations)
        {

            this.clustersHash = new Dictionary<IMLDataPair, int>();
            this.clusters = new List<ICollection<IMLDataPair>>();

            IList list = new List<IMLDataPair>();
            foreach (IMLDataPair pair in observations)
            {
                list.Add(pair);
            }
            KMeansUtil<IMLDataPair> kmc = new KMeansUtil<IMLDataPair>(k, list);
            kmc.Process();

            for (int i = 0; i < k; i++)
            {
                ICollection<IMLDataPair> cluster = kmc.Get(i);
                this.clusters.Add(cluster);

                foreach (IMLDataPair element in cluster)
                {
                    this.clustersHash[element] = i;
                }
            }
        }
        
        /// <summary>
        /// Get the speicified cluster. 
        /// </summary>
        /// <param name="n">The number.</param>
        /// <returns>The items in that cluster.</returns>
        public ICollection<IMLDataPair> Cluster(int n)
        {
            return this.clusters[n];
        }
        
        /// <summary>
        /// Get the cluster for the specified data pair. 
        /// </summary>
        /// <param name="o">The data pair to use..</param>
        /// <returns>The cluster the pair is in.</returns>
        public int Cluster(IMLDataPair o)
        {
            return this.clustersHash[o];
        }
        
        /// <summary>
        /// Determine if the specified object is in one of the clusters. 
        /// </summary>
        /// <param name="o">The object to check.</param>
        /// <param name="x">The cluster.</param>
        /// <returns>True if the object is in the cluster.</returns>
        public bool IsInCluster(IMLDataPair o, int x)
        {
            return Cluster(o) == x;
        }

        /// <summary>
        /// Put an object into the specified cluster. 
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="n">The cluster number.</param>
        public void Put(IMLDataPair o, int n)
        {
            this.clustersHash[o] = n;
            this.clusters[n].Add(o);
        }

        /// <summary>
        /// Remove an object from the specified cluster. 
        /// </summary>
        /// <param name="o">The object to remove.</param>
        /// <param name="n">The cluster to remove from.</param>
        public void Remove(IMLDataPair o, int n)
        {
            this.clustersHash[o] = -1;
            this.clusters[n].Remove(o);
        }
    }
}