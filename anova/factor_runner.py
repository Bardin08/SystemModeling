# Re-importing the necessary libraries after code execution state reset
import pandas as pd
import matplotlib as plt
import seaborn as sns
import statsmodels.api as sm
from statsmodels.formula.api import ols

# Load the updated dataset
updated_df = pd.read_csv("output.csv")

# Performing ANOVA with 'MeanLoadTime' as the dependent variable
anova_formula_mean_load_time = "MeanLoadTime ~ C(DurationSeconds) + C(DelayMilliseconds) + C(TotalEventsAmount) + C(ProcessorName) + MaxQueue + FailureChance + MeadQueueLength"
anova_model_mean_load_time = ols(anova_formula_mean_load_time, data=updated_df).fit()
anova_results_mean_load_time = sm.stats.anova_lm(anova_model_mean_load_time, typ=2)

# Performing ANOVA with 'MeadQueueLength' as the dependent variable
anova_formula_mead_queue_length = "MeadQueueLength ~ C(DurationSeconds) + C(DelayMilliseconds) + C(TotalEventsAmount) + C(ProcessorName) + MaxQueue + FailureChance + MeanLoadTime"
anova_model_mead_queue_length = ols(anova_formula_mead_queue_length, data=updated_df).fit()
anova_results_mead_queue_length = sm.stats.anova_lm(anova_model_mead_queue_length, typ=2)

# Plotting MeadQueueLength by ProcessorName
plt.figure(figsize=(10, 6))
sns.barplot(x='MeadQueueLength', y='ProcessorName', data=updated_df)
plt.title('MeadQueueLength by ProcessorName')
plt.tight_layout()
plt.show()

# Plotting MeadQueueLength by MaxQueue
plt.figure(figsize=(10, 6))
sns.barplot(x='MaxQueue', y='MeadQueueLength', data=updated_df)
plt.title('MeadQueueLength by MaxQueue')
plt.tight_layout()
plt.show()

# Plotting MeadQueueLength by FailureChance
plt.figure(figsize=(10, 6))
sns.barplot(x='FailureChance', y='MeadQueueLength', data=updated_df)
plt.title('MeadQueueLength by FailureChance')
plt.tight_layout()
plt.show()

# Plotting MeadQueueLength by MeanLoadTime
plt.figure(figsize=(10, 6))
sns.scatterplot(x='MeanLoadTime', y='MeadQueueLength', data=updated_df)
plt.title('MeadQueueLength by MeanLoadTime')
plt.tight_layout()
plt.show()