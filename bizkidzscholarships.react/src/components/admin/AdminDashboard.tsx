import { useEffect } from "react";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import { Link, useNavigate } from "react-router-dom";
import { UseAdminContext } from "../../contexts/AdminContext";
import { GetUserActivities, GetUserResults } from "../../services/AdminDataService";
import UserActivity from "./UserActivityTable";
import UserList from "./UserList";

function AdminDashboard() {
  const userAccountContext = UseUserAccountContext();
  const adminContext = UseAdminContext();
  const [cookie] = [userAccountContext.userCookie];
  const [setUserActivities] = [adminContext.setUserActivities];
  const [setUserResults] = [adminContext.setUserResults]
  const navigation = useNavigate();

  const getLogs = async () => {
    let userActivities = await GetUserActivities();

    if (userActivities != null) {
      setUserActivities(userActivities);
    } else {
      setUserActivities([]);
    }
  };
  
  const getUsers = async () => {
    let results = await GetUserResults();

    setUserResults(results);
  }

  useEffect(() => {
    if (!cookie.roles.includes("Admin")){
      navigation("/access-denied");
    }

    //getLogs();
    //getUsers();
    
  }, []);
  
  return (
      <>
          <div className="container">
              <div className="row">
                  <div className="col">
                      <Link to="users">
                          <div className="card bg-primary rounded-3">
                              <img className="card-img-top admin-card-img" src={"https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/biz-kidz-usa-logo.avif"}></img>
                              <div className="card-body">
                                <h3>Users</h3>
                              </div>
                          </div>
                      </Link>
                  </div>

                  <div className="col">
                      <Link to="submissions">
                          <div className="card bg-secondary rounded-3">
                              <img className="card-img-top admin-card-img" src={"https://bizkidz-task-bucket.s3.us-east-2.amazonaws.com/static/biz-kidz-usa-logo.avif"}></img>
                              <div className="card-body">
                                  <h3>Submissions</h3>
                              </div>
                          </div>
                      </Link>
                  </div>

                  <div className="col">
                      <Link to="tasks">
                          <div className="card bg-success h-100 rounded-3">
                              <div className="card-img-top image-slot d-flex text-white align-items-center justify-content-center admin-card-img">
                                  <i className="bi bi-card-checklist" style={{ fontSize: "12rem" }}></i>
                              </div>
                              <div className="card-body">
                                  <h3>Tasks</h3>
                              </div>
                          </div>
                      </Link>
                  </div>
              </div>
              <div className="row">
                  <div className="col">
                    
                  </div>
              </div>
          </div>
        </>
  );
}

export default AdminDashboard;

