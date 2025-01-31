﻿// <copyright file="CoachProfileForm.cs" company="Adam J. Smith at NWTC">
//     Copyright 2018 Smithbucks Computing (Adam J. Smith, radarsmith83@gmail.com)
// </copyright>
namespace CoachConnect
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Definition of the <see cref="CoachProfileForm"/> class.
    /// </summary>
    public partial class CoachProfileForm : Form
    {
        /// <summary>
        /// A list to store available courses for the listed department
        /// and provide a binding data source to the Available Courses list box.
        /// </summary>
        private BindingList<ViewDepartmentCours> unselectedCourseList;

        /// <summary>
        /// A list to store the current coach's courses that are saved in the database.
        /// </summary>
        private BindingList<ViewCoachCours> coachSelectedCourseList;

        /// <summary>
        /// A list to store current selected courses for the coach
        /// and provide a binding data source to the Available Courses list box.
        /// </summary>
        private BindingList<ViewCoachCours> currentSelectedCourseList;

        /// <summary>
        /// A boolean to determine whether the current coach profile has been modified.
        /// </summary>
        private bool isCoachModified = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoachProfileForm"/> class.
        /// </summary>
        public CoachProfileForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Maximizes Form and displays the users with load event of the form.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CoachAdminFormLoad(object sender, EventArgs e)
        {
            this.DisplaySupervisors();
            this.DisplayCoaches();
            this.GetCoachSelectedCourses();
            this.DisplaySelectedCourses();
            this.DisplayDepartments();
        }

        /// <summary>
        /// Method to display the list of users
        /// </summary>
        private void DisplayCoaches()
        {
            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    // Query coach table in database and returns the list of the users in ascending order according to last name
                    var coachQuery = from coaches in context.Coaches
                        orderby coaches.LastName
                        select coaches;

                    // Convert query results to list
                    List<Coach> coachList = coachQuery.ToList();

                    // Set combo box data sources and update data member settings
                    this.cbxChooseCoach.DataSource = coachList;
                    this.cbxChooseCoach.ValueMember = "CoachID";
                    this.cbxChooseCoach.DisplayMember = "DisplayName";
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to display the list of supervisors in a combo box
        /// </summary>
        private void DisplaySupervisors()
        {
            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    // Query user table in database and return the list of supervisors in ascending order according to last name
                    var supervisorQuery = from users in context.Users
                        where users.IsSupervisor
                        orderby users.LastName
                        select users;

                    List<User> supervisorList = supervisorQuery.ToList();

                    // Set combo box data source and update data member listings
                    this.cbxSupervisor.DataSource = supervisorList;
                    this.cbxSupervisor.ValueMember = "UserID";
                    this.cbxSupervisor.DisplayMember = "DisplayName";
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to display the list of users
        /// </summary>
        private void DisplayDepartments()
        {
            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    // Query user table in database and returns the list of the users in ascending order according to last name
                    var departmentQuery = from departments in context.Departments
                        orderby departments.DepartmentName
                        select departments;

                    // Convert query results to list
                    List<Department> departmentList = departmentQuery.ToList();

                    // Set combo box data source and update data member settings
                    this.cbxDepartment.DataSource = departmentList;
                    this.cbxDepartment.ValueMember = "DepartmentID";
                    this.cbxDepartment.DisplayMember = "DepartmentName";
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to obtain the current coach's list of courses.
        /// This list does not change until the coach profile is saved
        /// </summary>
        private void GetCoachSelectedCourses()
        {
            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    string coachId = this.cbxChooseCoach.SelectedValue.ToString();

                    var selectedCoursesQuery = from courses in context.ViewCoachCourses
                        where courses.CoachID == coachId
                        orderby courses.CourseName
                        select courses;

                    // Convert query results to lists and store in object fields
                    this.coachSelectedCourseList = new BindingList<ViewCoachCours>(selectedCoursesQuery.ToList());
                    this.currentSelectedCourseList = new BindingList<ViewCoachCours>(selectedCoursesQuery.ToList());
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to display the list of unselected courses
        /// </summary>
        private void DisplayUnselectedCourses()
        {
            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    string departmentId = this.cbxDepartment.SelectedValue.ToString();

                    var departmentCoursesQuery = from courses in context.ViewDepartmentCourses
                        where courses.DepartmentID == departmentId
                        orderby courses.CourseName
                        select courses;

                    var currentSelectedCoursesQuery = from courses in this.currentSelectedCourseList
                        orderby courses.CourseName
                        select courses.CourseID;

                    var unselectedCoursesQuery = from deptCourses in departmentCoursesQuery
                        where !currentSelectedCoursesQuery.Contains(deptCourses.CourseID)
                        select deptCourses;

                    // Convert query results to list
                    this.unselectedCourseList =
                        new BindingList<ViewDepartmentCours>(unselectedCoursesQuery.ToList());

                    // Set list box data source and update data member settings
                    this.lstUnselectedCourses.DataSource = this.unselectedCourseList;
                    this.lstUnselectedCourses.ValueMember = "CourseID";
                    this.lstUnselectedCourses.DisplayMember = "CourseName";
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Method to display the list of selected courses for the current coach
        /// </summary>
        private void DisplaySelectedCourses()
        {
            // Reset the selected list box
            this.lstSelectedCourses.DataSource = this.currentSelectedCourseList;
            this.lstSelectedCourses.ValueMember = "CourseID";
            this.lstSelectedCourses.DisplayMember = "CourseName";
        }

        /// <summary>
        /// Clear all data fields
        /// </summary>
        private void ClearAllFields()
        {
            // Clearing check boxes and text boxes.
            this.txtID.Clear();
            this.txtFirstName.Clear();
            this.txtLastName.Clear();
            this.txtMiddleName.Clear();
            this.txtDisplayName.Clear();
            this.txtEmail.Clear();
            this.txtPhone.Clear();
            this.chkActive.Checked = false;
            this.cbxSupervisor.SelectedIndex = -1;
        }

        /// <summary>
        /// Populates the combo boxes and text boxes with the selected coach's information.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CbxChooseCoach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxChooseCoach.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                // Call method to determine whether current coach profile has been modified
                this.CheckForModifiedProfile();

                // If the current coach profile is modified, display an alert
                if (this.isCoachModified)
                {
                    DialogResult cancelChoice = MessageBox.Show(
                        @"This action will remove all current changes.  Do you want to continue?",
                        @"Cancel", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Warning);

                    if (cancelChoice == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            try
            {
                using (var context = new db_sft_2172Entities())
                {
                    // Obtain the selected coach ID from the combo box
                    string coachId = this.cbxChooseCoach.SelectedValue.ToString();

                    // Find the coach in the database.
                    var coachQuery = from coach in context.Coaches
                        where coach.CoachID.Equals(coachId)
                        select coach;

                    // If the query returns a a user, display the corresponding info in the form
                    if (coachQuery.Any())
                    {
                        var coachResult = coachQuery.FirstOrDefault();

                        // If a query result is found, update fields and course lists
                        if (coachResult != null)
                        {
                            this.txtID.Text = coachResult.CoachID;
                            this.txtFirstName.Text = coachResult.FirstName;
                            this.txtLastName.Text = coachResult.LastName;
                            this.txtMiddleName.Text = coachResult.MiddleName;
                            this.txtDisplayName.Text = coachResult.DisplayName;
                            this.txtEmail.Text = coachResult.Email;
                            this.txtPhone.Text = coachResult.Phone;
                            this.cbxSupervisor.SelectedValue = coachResult.SupervisorID;
                            this.chkActive.Checked = coachResult.IsActive;

                            this.GetCoachSelectedCourses();
                            this.DisplaySelectedCourses();

                            // Reset modified flag
                            this.isCoachModified = false;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Update available courses when the Department combo box value is changed.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void CbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbxDepartment.SelectedIndex == -1)
            {
                return;
            }

            this.DisplayUnselectedCourses();
        }

        /// <summary>
        /// Add button clears all text boxes and check boxes so user can enter new information.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BtnAddCoachClick(object sender, EventArgs e)
        {
            // Call method to determine whether current coach profile has been modified
            this.CheckForModifiedProfile();

            // If the current coach profile is modified, display an alert
            if (this.isCoachModified)
            {
                DialogResult cancelChoice = MessageBox.Show(
                    @"This action will remove all current changes.  Do you want to continue?",
                    @"Cancel", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (cancelChoice == DialogResult.No)
                {
                    return;
                }
            }

            // Clear all fields and enable text ID for new user input
            this.ClearAllFields();
            this.txtID.Enabled = true;

            // Clear out course lists (new user should have no courses yet)
            this.coachSelectedCourseList = new BindingList<ViewCoachCours>();
            this.currentSelectedCourseList = new BindingList<ViewCoachCours>();
            
            this.DisplaySelectedCourses();
            this.DisplayUnselectedCourses();

            // Set modified flag to ensure user has a chance to save data
            this.isCoachModified = true;
        }

        /// <summary>
        /// Submit button sends the added or updated info to the database.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BtnSubmitClick(object sender, EventArgs e)
        {
            List<ViewCoachCours> addList = new List<ViewCoachCours>();
            List<ViewCoachCours> removeList = new List<ViewCoachCours>();

            // Loop through selected courses and determine whether selected courses
            // need to be added to the list
            foreach (ViewCoachCours selectedCourse in this.currentSelectedCourseList)
            {
                bool isCourseSelected = false;

                foreach (ViewCoachCours coachCourse in this.coachSelectedCourseList)
                {
                    if (coachCourse.CourseID.Equals(selectedCourse.CourseID))
                    {
                        isCourseSelected = true;
                        break;
                    }
                }

                if (!isCourseSelected)
                {
                    addList.Add(selectedCourse);
                }
            }

            // Loop through the originally selected courses and determine whether
            // any were removed
            foreach (ViewCoachCours coachCourse in this.coachSelectedCourseList)
            {
                bool isCourseSelected = false;

                foreach (ViewCoachCours selectedCourse in this.currentSelectedCourseList)
                {
                    if (coachCourse.CourseID.Equals(selectedCourse.CourseID))
                    {
                        isCourseSelected = true;
                        break;
                    }
                }

                if (!isCourseSelected)
                {
                    removeList.Add(coachCourse);
                }
            }

            try
            {
                // Query updates the user in the database
                using (var context = new db_sft_2172Entities())
                {
                    string coachId = this.txtID.Text;
                    var coachQuery = from coach in context.Coaches
                        where coach.CoachID.Equals(coachId)
                        select coach;

                    if (coachQuery.Any())
                    {
                        var coachResult = coachQuery.FirstOrDefault();
                        coachResult.FirstName = this.txtFirstName.Text;
                        coachResult.MiddleName = this.txtMiddleName.Text;
                        coachResult.LastName = this.txtLastName.Text;
                        coachResult.DisplayName = this.txtDisplayName.Text;
                        coachResult.Phone = this.txtPhone.Text;
                        coachResult.Email = this.txtEmail.Text;
                        coachResult.IsActive = this.chkActive.Checked;

                        coachResult.SupervisorID = this.cbxSupervisor.SelectedIndex == -1
                            ? string.Empty
                            : this.cbxSupervisor.SelectedValue.ToString();

                        foreach (ViewCoachCours course in addList)
                        {
                            CoachCourse newCourse = new CoachCourse
                            {
                                CoachID = this.txtID.Text,
                                CourseID = course.CourseID,
                                Active = true
                            };

                            context.CoachCourses.Add(newCourse);
                        }

                        foreach (ViewCoachCours course in removeList)
                        {
                            CoachCourse dropCourse = new CoachCourse
                            {
                                CoachID = this.txtID.Text,
                                CourseID = course.CourseID
                            };

                            context.CoachCourses.Attach(dropCourse);
                            context.CoachCourses.Remove(dropCourse);
                        }

                        context.SaveChanges();
                        MessageBox.Show(@"Coach Profile Updated");

                        this.GetCoachSelectedCourses();
                        this.DisplaySelectedCourses();
                        this.DisplayUnselectedCourses();
                    }
                    else
                    {
                        Coach newCoach = new Coach
                        {
                            CoachID = this.txtID.Text,
                            FirstName = this.txtFirstName.Text,
                            MiddleName = this.txtMiddleName.Text,
                            LastName = this.txtLastName.Text,
                            DisplayName = this.txtDisplayName.Text,
                            Phone = this.txtPhone.Text,
                            Email = this.txtEmail.Text,
                            IsActive = this.chkActive.Checked,
                            SupervisorID =
                                this.cbxSupervisor.SelectedIndex == -1
                                    ? string.Empty
                                    : this.cbxSupervisor.SelectedValue.ToString()
                        };

                        foreach (ViewCoachCours course in addList)
                        {
                            CoachCourse newCourse = new CoachCourse
                            {
                                CoachID = this.txtID.Text,
                                CourseID = course.CourseID,
                                Active = true
                            };

                            context.CoachCourses.Add(newCourse);
                        }

                        foreach (ViewCoachCours course in removeList)
                        {
                            CoachCourse dropCourse = new CoachCourse
                            {
                                CoachID = this.txtID.Text,
                                CourseID = course.CourseID
                            };

                            context.CoachCourses.Attach(dropCourse);
                            context.CoachCourses.Remove(dropCourse);
                        }

                        context.Coaches.Add(newCoach);
                        context.SaveChanges();

                        MessageBox.Show(@"Coach Profile Added");

                        // If save is successful, update the coach list and display the new coach profile
                        this.DisplayCoaches();
                        this.cbxChooseCoach.SelectedValue = newCoach.CoachID;
                        this.txtID.Enabled = false;

                        this.GetCoachSelectedCourses();
                        this.DisplaySelectedCourses();
                        this.DisplayUnselectedCourses();
                    }
                }
            }
            catch (DbUpdateException dbUEx)
            {
                MessageBox.Show(dbUEx.InnerException != null ? dbUEx.InnerException.Message : dbUEx.Message);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Close button closes the current window.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Call method to determine whether current coach profile has been modified
            this.CheckForModifiedProfile();

            // If the current coach profile is modified, display an alert
            if (this.isCoachModified)
            {
                DialogResult cancelChoice = MessageBox.Show(
                    @"Closing this window will remove all changes.  Do you want to continue?",
                    @"Cancel", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (cancelChoice == DialogResult.No)
                {
                    return;
                }
            }

            this.Close();
        }

        /// <summary>
        /// Remove button deletes the selected course from the Selected Course list box,
        /// then refreshes the Unselected and Selected list boxes.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BtnRemoveCourse_Click(object sender, EventArgs e)
        {
            if (lstSelectedCourses.SelectedIndex == -1)
            {
                MessageBox.Show(@"Please select a course to remove.");
            }
            else
            {
                this.currentSelectedCourseList.Remove((ViewCoachCours)this.lstSelectedCourses.SelectedItem);
                this.DisplayUnselectedCourses();
                this.DisplaySelectedCourses();

                this.isCoachModified = true;
            }
        }

        /// <summary>
        /// Select button adds the selected course to the Selected Course list box,
        /// then refreshes the Unselected and Selected list boxes.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void BtnSelectCourse_Click(object sender, EventArgs e)
        {
            if (lstUnselectedCourses.SelectedIndex == -1)
            {
                MessageBox.Show(@"Please select a course to add.");
            }
            else
            {
                ViewDepartmentCours selectedCourse = (ViewDepartmentCours)lstUnselectedCourses.SelectedItem;

                Coach selectedCoach = (Coach)cbxChooseCoach.SelectedItem;

                ViewCoachCours newCourse = new ViewCoachCours()
                {
                    CoachID = selectedCoach.CoachID,
                    CourseID = selectedCourse.CourseID,
                    CourseName = selectedCourse.CourseName
                };

                this.currentSelectedCourseList.Add(newCourse);

                this.DisplayUnselectedCourses();
                this.DisplaySelectedCourses();

                this.isCoachModified = true;
            }
        }

        /// <summary>
        /// Determine whether the current coach profile has been modified.
        /// If yes, update a boolean value stored in the Form object.
        /// </summary>
        private void CheckForModifiedProfile()
        {
            try
            {
                // Query updates the user in the database
                using (var context = new db_sft_2172Entities())
                {
                    string coachId = this.txtID.Text;
                    var coachQuery = from coach in context.Coaches
                        where coach.CoachID.Equals(coachId)
                        select coach;

                    if (coachQuery.Any())
                    {
                        var coachResult = coachQuery.FirstOrDefault();

                        if (coachResult.FirstName != this.txtFirstName.Text ||
                            coachResult.MiddleName != this.txtMiddleName.Text ||
                            coachResult.LastName != this.txtLastName.Text ||
                            coachResult.DisplayName != this.txtDisplayName.Text ||
                            coachResult.Phone != this.txtPhone.Text ||
                            coachResult.Email != this.txtEmail.Text ||
                            coachResult.IsActive != this.chkActive.Checked ||
                            coachResult.SupervisorID != this.cbxSupervisor.SelectedValue.ToString())
                        {
                            this.isCoachModified = true;
                        }
                    }
                }
            }
            catch (DbUpdateException dbUEx)
            {
                MessageBox.Show(dbUEx.InnerException != null ? dbUEx.InnerException.Message : dbUEx.Message);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.InnerException != null ? sqlEx.InnerException.Message : sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}