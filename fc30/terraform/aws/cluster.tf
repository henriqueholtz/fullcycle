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

resource "aws_cloudwatch_log_group" "log" {
  name = "/aws/eks-terraform-course/${var.prefix}-${var.cluster_name}/cluster"
  retention_in_days = var.retention_in_days
}

resource "aws_eks_cluster" "cluster" {
  name = "${var.prefix}-${var.cluster_name}"
  role_arn = aws_iam_role.cluster-role.arn
  enabled_cluster_log_types = ["api","audit"]
  vpc_config {
      subnet_ids = aws_subnet.subnets[*].id # var.subnet_ids 
      security_group_ids = [aws_security_group.sec-group.id]
  }
  depends_on = [
    aws_cloudwatch_log_group.log,
    aws_iam_role_policy_attachment.cluster-AmazonEKSVPCResourceController,
    aws_iam_role_policy_attachment.cluster-AmazonEKSClusterPolicy,
  ]
}