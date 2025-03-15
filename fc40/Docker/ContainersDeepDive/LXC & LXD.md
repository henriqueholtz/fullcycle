## LXC & LXD

1. Execute an ubuntu container: `docker run -d --rm --privileged=true --name=ubuntu ubuntu:22.04 sleep infinity`
2. Access the container: `docker exec -it ubuntu bash`
3. Install needed packages: `apt update && apt install -y sudo snapd`
4. Fix to run snap correctly [ref](https://github.com/microsoft/WSL/issues/5126#issuecomment-653715201)

   ```
   sudo apt-get update && sudo apt-get install -yqq daemonize dbus-user-session fontconfig

   sudo daemonize /usr/bin/unshare --fork --pid --mount-proc /lib/systemd/systemd --system-unit=basic.target

   exec sudo nsenter -t $(pidof systemd) -a su - $LOGNAME

   snap version

   ```

5. Install lxd: `sudo snap install lxd`
6. Init lxd: `lxd init --auto`
7. List images: `lxc image list images:alpine arch={arm64 or amd64}`
8. Launch: `lxc launch images:alpine/3.18/{arm64 or amd64} mycontainer` or `lxc launch images:{fingerprintId} mycontainer`
9. List containers: `lxc list`
10. Exec: `lxc exec mycontainer -- /bin/sh`
11. Show lxd's processes `ps aux | grep lxd`
12. Stop a container: `lxc stop mycontainer`
13. Delete a container: `lxc delete mycontainer`
