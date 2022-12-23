resource "aws_security_group" "sec-group" {
    vpc_id = aws_vpc.new-vpc.id

    egress {
        from_port = 0
        to_port = 0 # All ports
        protocol = "-1" # All protocols
        cidr_blocks = ["0.0.0.0/0"] # All IPs
        prefix_list_ids = []
    }

    tags = {
        Name = "${var.prefix}-sec-group"
    }
}