import os
import json
import pandas as pd

# Directory path to the JSON files
directory_path = "experiments"


# Function to read and parse JSON files
def read_and_parse_json_files(directory_path):
    all_data = []
    for filename in sorted(os.listdir(directory_path)):
        if filename.endswith(".json"):
            file_path = os.path.join(directory_path, filename)
            with open(file_path, 'r') as file:
                data = json.load(file)
                config = data["SimulationConfig"]["config"]
                duration_seconds = config["DurationSeconds"]
                delay_milliseconds = config["EventsGenerator"]["Delay"]
                total_events_amount = config["EventsGenerator"]["TotalEventsAmount"]
                max_queue = config["Validation"]["MaxQueue"]
                failure_chance = config["Validation"]["ValidationFailureChance"]

                for result in data["Result"]:
                    processor = result["Processor"]
                    processor_name = processor["ProcessorName"]
                    mead_queue_length = processor["MeadQueueLength"]
                    mean_load_time = processor["MeanLoadTime"]
                    router = result["Router"]
                    success_events = router["SuccessEvents"]
                    failed_events = router["FailedEvents"]

                    all_data.append({
                        "DurationSeconds": duration_seconds,
                        "DelayMilliseconds": delay_milliseconds,
                        "TotalEventsAmount": total_events_amount,
                        "ProcessorName": processor_name,
                        "MaxQueue": max_queue,
                        "FailureChance": failure_chance,
                        "MeadQueueLength": mead_queue_length,
                        "MeanLoadTime": mean_load_time,
                        "SuccessEvents": success_events,
                        "FailedEvents": failed_events
                    })
    return all_data


# Read and parse the JSON files
data = read_and_parse_json_files(directory_path)

# Create a DataFrame and save to CSV
df = pd.DataFrame(data)
df.to_csv("output.csv", index=False)
