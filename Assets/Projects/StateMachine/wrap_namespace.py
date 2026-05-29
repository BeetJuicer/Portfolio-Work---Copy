import os

namespace = "StateMachine"
folder = "."  # change this

for root, dirs, files in os.walk(folder):
    for file in files:
        if file.endswith(".cs"):
            path = os.path.join(root, file)
            with open(path, "r") as f:
                content = f.read()
            if f"namespace {namespace}" not in content:
                wrapped = f"namespace {namespace}\n{{\n{content}\n}}"
                with open(path, "w") as f:
                    f.write(wrapped)
                print(f"Wrapped: {path}")