# AdPasswordReset

Active Directory tool for super-users to reset password for other users in same OU.

This tool lists all users in running user's OU, including sub-OU's. Disabled users are not displayed.

## Usage

To allow a "Super"user to change others users password:

* Create a group in AD
* For the OU in question:
* Delegate "Reset user passwords and force password cange at next logon" to this group
* Add the "Super"user(s) to this group.
