import { useEffect } from "react";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import { useNavigate } from "react-router-dom";
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

    getLogs();
    getUsers();
    
  }, []);
  
  return (
      <>
          <div className="dashboard container">
            <div className="row justify-content-center">
              <div className="col-12">
                <h1 className="text-center">BizKidz Admin Dashboard</h1>
              </div>
            </div>

            <div className="row justify-content-center" style={{ "height": "100px" }}>
              <div className="col d-flex justify-content-center">
                <button type="button" className="btn btn-warning border-dark btn-lg w-100">Configuration ⚙</button>
              </div>

              <div className="col d-flex justify-content-center">
                <button type="button" className="btn btn-primary border-black border-2 btn-lg w-100">Users 🧍</button>
              </div>

              <div className="col d-flex justify-content-center">
                <button type="button" className="btn btn-danger border-black border-2 btn-lg w-100">Tasks ✅</button>
              </div>
            </div>

            <div className="row justify-content-center align-items-center mt-5 mb-1">
              <div className="col-12 col-md-8">
                <h2 className="text-center">Recent Activity</h2>
                <UserActivity />
              </div>
            
            </div>
            
            
            <div className="row mt-4">
              <div className="col col-lg-6 col-xs-12">
                <h2>Users</h2>
                <UserList />
              </div>
              
            </div>
            
          </div>
        </>
  );
}

export default AdminDashboard;

