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

resource "aws_iam_role" "cluster-role" {
    name = "${var.prefix}-${var.cluster_name}"
    assume_role_policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "eks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}   
POLICY
}

resource "aws_iam_role_policy_attachment" "cluster-AmazonEKSVPCResourceController" {
  role = aws_iam_role.cluster-role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSVPCResourceController"
}

resource "aws_iam_role_policy_attachment" "cluster-AmazonEKSClusterPolicy" {
  role = aws_iam_role.cluster-role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
}