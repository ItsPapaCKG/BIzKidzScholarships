import { useEffect } from "react";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import { useNavigate } from "react-router-dom";
import { UseAdminContext } from "../../contexts/AdminContext";
import { GetUserActivities } from "../../services/AdminDataService";
import UserActivity from "./UserActivityTable";

function AdminDashboard() {
  const userAccountContext = UseUserAccountContext();
  const adminContext = UseAdminContext();
  const [cookie] = [userAccountContext.userCookie];
  const [setUserActivities] = [adminContext.setUserActivities];
  const navigation = useNavigate();

  const getLogs = async () => {
    let userActivities = await GetUserActivities();

    if (userActivities != null) {
      setUserActivities(userActivities);
    } else {
      setUserActivities([]);
    }
  };

  useEffect(() => {
    if (!cookie.roles.includes("Admin")){
      navigation("/access-denied");
    }

    getLogs();
    
  }, []);
  
  return (
      <>
          <div className="dashboard container">
            <div className="row justify-content-center">
              <div className="col-12">
                <h1 className="text-center">BizKidz Scholarship Dashboard</h1>
              </div>
            </div>

            <div className="row justify-content-center align-items-center">
              <div className="col-12 col-md-8">
                <UserActivity />
              </div>
            
            </div>
            
            
            <div className="row mt-4">
              <div className="col col-lg-6 col-xs-12">
                
              </div>
              
            </div>
            
          </div>
        </>
  );
}

export default AdminDashboard;

