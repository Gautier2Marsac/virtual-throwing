#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import os
import pandas as pd


base_dir      = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
data_dir      = os.path.join(base_dir, 'data')
selected_file = os.path.join(data_dir, 'combined_selected_conditions.csv')
output_file   = os.path.join(data_dir, 'analyse_lancers_table.csv')


df = pd.read_csv(selected_file)

# S’assurer que RealTime est propre
df['RealTime'] = df['RealTime'].astype(str).str.strip()

# Renommer la colonne existante pour correspondre au format précédent
df = df.rename(columns={'distance_impacts': 'DistanceImpact'})

#calcul de l'erreur par rapport à la cible 
df['error_to_target'] = (df['DistanceImpact'] - df['target_distance']).abs()

#la séléction des colonnes et écriture
cols = [
    'nom',
    'condition',
    'target_distance',
    'DistanceImpact',
    'error_to_target',
    'RealTime'
]
df[cols].to_csv(output_file, index=False)

print(f"Table d'analyse générée (sans bruts) : {output_file}")
