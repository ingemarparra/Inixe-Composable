// -----------------------------------------------------------------------
// <copyright file="CommandPermissionAttribute.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CommandPermissionAttribute : Attribute
    {
        private readonly string permissionName;

        public CommandPermissionAttribute(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new ArgumentException("Invalid permission name", nameof(permissionName));
            }

            this.permissionName = permissionName;
        }

        public string PermissionName
        {
            get
            {
                return this.permissionName;
            }
        }
    }
}
