# AdPasswordReset

Active Directory tool for super-users. This tool lists all users in running users OU, including sub OU's but not disabled users.

## Usage

Use the "Delegation of Control Wizard" in Active Directory.

**To allow a "Super" user to change others users password:**

1. Create a group in AD
2. For the OU in question: Delegate "Reset user passwords and force password cange at next logon" to this group
3. Add the "Super" user(s) to this group.

**To give a "Super" user rights to unlock account(s):**

1. Create a group in AD (can be the same as above)
2. For the OU in question:
a) On the Tasks to Delegate dialog box, click Create a custom task to delegate, and then click Next.
b) On the Active Directory Object Type dialog box, click Only the following objects in the folder:. In the list, click User objects (the last entry in the list), and then click Next.
c) On the Permissions dialog box, click to clear the General check box, and then click to select the Property-specific check box. In the Permissions list, click to select the Read lockoutTime check box, click to select the Write lockoutTime check box, and then click Next.
d) On the Completing the Delegation of Control Wizard dialog box, click Finish.
