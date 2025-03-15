# Docker: Containers Deep Dive - FullCycle 4.0

Create a namespace and a cgroup, and attaching them to the a process (bash) using docker.

We'll work with 2 terminals:

- Terminal 1: Initial Setup and starting a `bash` process with a new namespace
- Terminal 2: Ubuntu - general

**Note**: The CPU and memory limit isn't working because of cgroup2 [see more details here](https://forum.code.education/forum/topico/arquivo-cpustat-nao-encontrado-5048/)

1. [Terminal 1] Execute an ubuntu container: `docker run -d --rm --privileged=true --name=ubuntu ubuntu:22.04 sleep infinity`
2. [Terminal 1] Access the container: `docker exec -it ubuntu bash`
3. [Terminal 1] Install the needed packages: `apt update && apt install -y sudo psmisc stress`
4. [Terminal 1] Create the namespace using "unshare": `sudo unshare -p -m -n -f --mount-proc bash`
5. [Terminal 2] Access the container: `docker exec -it ubuntu bash`
6. [Terminal 2] List the processes which has "unshare": `ps -ef | grep unshare`
7. [Terminal 2] Show tree of main process showed at 6° step: `pstree -p {PID}`
8. [Terminal 2] Create a folder to the new cgroup (let's call "mycgroup"): `sudo mkdir /sys/fs/cgroup/mycgroup`
9. [Terminal 2] Give the permissions to the new folder: `chmod -R 777 /sys/fs/cgroup/mycgroup/`
10. [Terminal 2] Adding CPU limit: `echo "50000 100000" | sudo tee /sys/fs/cgroup/mycgroup/cpu.max`
11. [Terminal 2] Adding Memory limit: `echo "100M" | sudo tee /sys/fs/cgroup/mycgroup/memory.max`
12. [Terminal 2] Mount the new cgroup as csgroup2: `sudo mount -t cgroup2 none /sys/fs/cgroup/mycgroup`
13. [Terminal 2] Attach the process to the bash (which is in the new namespace) to the new cgroup: `echo "{PID}" | sudo tee /sys/fs/cgroup/mycgroup/cgroup.procs`
14. [Terminal 1] Execute load/stress test: `stress --cpu 2 --vm 1 --vm-bytes 80M --timeout 30`
15. [Terminal 2] Show cgroup's cpu.stat: `cat /sys/fs/cgroup/mycgroup/cpu.stat`
