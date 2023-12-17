import pandas as pd
import itertools

duration_seconds = [10, 20, 30]
delay_milliseconds = [1, 5, 10]
total_events_amount = [10, 50, 100]
queue_size_threshold = [100, 500, 1000]
failure_chance = [0.01, 0.05, 0.1]

experiment_combinations = list(
    itertools.product(duration_seconds, delay_milliseconds, total_events_amount, queue_size_threshold, failure_chance))

experiment_matrix = pd.DataFrame(
    experiment_combinations,
    columns=["DurationSeconds", "DelayMilliseconds", "TotalEventsAmount", "MaxQueue", "FailureChance"])

experiment_matrix.to_clipboard()
