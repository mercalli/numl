﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math;
using numl.Model;

namespace numl.Unsupervised
{
    public class PCA
    {
        public Vector Eigenvalues { get; private set; }
        public Matrix Eigenvectors { get; private set; }
        public Matrix X { get; private set; }
        public Matrix Reduced { get; private set; }
        public void Generate(Matrix matrix)
        {
            // generate centered matrix
            // (using a copy since centering is in place)
            X = matrix
                    .Copy()
                    .Center(VectorType.Column);

            // compute eigen-decomposition
            // of covariance matrix
            var eigs = X.Covariance().Eigs();
            Eigenvalues = eigs.Item1;
            Eigenvectors = eigs.Item2;
        }

        public void Generate(Description desciption, IEnumerable<object> examples)
        {
            // generate data matrix
            var x = examples.ToMatrix(desciption);
            Generate(x);
        }

        public Matrix Reduce(int maxdim)
        {
            if(maxdim < 1) throw new InvalidOperationException("Cannot reduce to less than 1 dimension!");
            if (X == null || Eigenvalues == null || Eigenvectors == null)
                throw new InvalidOperationException("Cannot reduce until pca data has been generated");

            // get columns in reverse order
            // and stuff into matrix
            Matrix reduc = Eigenvectors
                                .Reverse(VectorType.Column)
                                .Take(maxdim)
                                .ToMatrix();

            Reduced = reduc * X.T;

            return Reduced;
        }
    }
}