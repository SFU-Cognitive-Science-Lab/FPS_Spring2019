# FPS_Spring2019
3d version of VR_Fall2018_1

This repository represents the original source code used for the Standard Computer Interface condition of the experiment.
One of the 2018 or 2019 versions of Unity should work for building the project.

See https://github.com/SFU-Cognitive-Science-Lab/VR_Fall2018_1 for the original VR game that this condition is based on.
The main changes where the addition of code to all the experiment to be controlled by a standard game controller.

To avoid resource issues on the host, it is recommended to build an .exe and run the experiment using the .exe.

Before running this either via the Unity editor or the stand alone .exe make sure to copy the experiments.config.txt
file to the Desktop directory. The VR program bootstraps itself by looking for this file. 

The experiment depends on an additional configuration file, arrangements.json, that contains all of the conditions for the
experiment. When running the experiment from the editor, the path to arrangements.json can probably be left as-is. 
However, when running from a stand alone .exe file the full path to the file should be put in the experiments.config.txt file.

The experiment depends on the NewtonSoft json library to parse the arrangements.json file.

An example of our running procedures can be found here: [Running Procedures - 3D Condition](https://github.com/SFU-Cognitive-Science-Lab/FPS_Spring2019/blob/vr/Procedures%20for%20Running%20CategoryVR%20-%203D.pdf)

