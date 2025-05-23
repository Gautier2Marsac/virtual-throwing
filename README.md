# Virtual Throwing & Distance Perception in AR

## Objective

This project investigates how haptic feedback — specifically mass and vibration — can improve distance perception during virtual throwing tasks in augmented reality (AR). The goal is to determine whether tactile cues help users better estimate egocentric distances, which are often underestimated in AR environments.

## Experimental Setup

- Hardware: Oculus Quest 2 headset and controllers
- Unity Scene: Designed from scratch using Unity's XR Toolkit, including:
  - XR Rig configuration for head and hand tracking
  - Virtual ball throwing mechanism
  - Toggleable vibration system (left/right hand via A/X buttons)
  - Collision detection for precise impact logging
  - CSV logging of each throw’s metadata:
    - Real-time and elapsed time
    - Start and impact positions
    - Throwing velocity
    - Distance to the target
    - Active condition (mass, vibration, both, or control)

## Data Collection

- Participants: 17 total
- Protocol:
  - Each participant completed 8 experimental conditions at three distances (4 m, 7 m, 12 m)
  - 24 labeled throws per participant were selected (3 per condition per distance)
  - Throws used for warm-up or trials were excluded via a manual mapping file
- Conditions:
  - Control (no mass, no vibration)
  - Vibration only (10 Hz or 20 Hz)
  - Mass only (100 g or 200 g)
  - Combined mass and vibration

## Statistical Analysis

All analyses were performed in Python using pandas, scipy, seaborn, and pingouin.

1. Preprocessing:
   - Concatenation and filtering of raw CSVs
   - Manual selection of valid throws using combined_selected_conditions.csv

2. Error Metric:
   - Absolute error was calculated as |DistanceImpact - TargetDistance|

3. Normality Testing:
   - Shapiro–Wilk test was applied to the error distribution of each condition
   - At least one condition failed the normality test, so ANOVA was not used

4. Friedman Test:
   - Non-parametric test for repeated measures
   - Significant differences were found across the 8 conditions (p < 0.001)

5. Post-hoc Analysis:
   - Wilcoxon signed-rank test for pairwise comparisons
   - Bonferroni correction was applied to control the family-wise error rate

## Visualization

- Boxplots: Show the distribution of absolute errors across conditions for each distance
- Bar plots: Show mean error and standard deviation across conditions

These graphs helped identify which combinations of mass and vibration led to improved precision.

## Results Summary

- Mass improved precision across all distances
- Vibration alone had a limited effect
- Combining mass and vibration produced the best results overall
- Participants reported improved control and realism with haptic feedback

## Repository Structure

├── Assets/
│ └── Scripts/ → Unity C# scripts (logger, vibration toggle, etc.)
├── Scenes/ → Unity scene for virtual throwing
├── data/
│ ├── raw/ → 17 CSV files (one per participant)
│ ├── combined_selected_conditions.csv
│ └── analyse_lancers_table.csv
├── analysis/
│ ├── filter_experiment_data.py
│ ├── analyse_lancers.py
│ └── stats_analysis.ipynb (optional)
├── README.md


## Notes

- The Unity scene is functional and ready for experimentation
- Python scripts are documented and reusable
- All statistical steps are automated and reproducible
- Designed as part of the PRONTO project at IMT Atlantique (2025)
