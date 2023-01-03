# Ansible

By default Ansible connect with SSH.

https://github.com/devfullcycle/fc-ansible

### Commands

- `ansible -i hosts-ping-local all -m ping`
- `cd /root/ansible/`
- `ansible -i hosts all -m ping`

### Run

- `docker-compose up -d`

### Configure keys

- `docker exec -it node1 bash` and/or `docker exec -it node2 bash`

  - `service ssh start`

- `docker exec -it control bash`
  - `ssh-keygen` (generate a key without passphrase)
  - `ssh-copy-id root@node1` => send the public key to `root@node1` (require the root password)
  - `ssh root@node1` => login without password
  - `exit` (return back to the control)
  - `cd /root/asible`
  - `ansible -i hosts all -m ping`
