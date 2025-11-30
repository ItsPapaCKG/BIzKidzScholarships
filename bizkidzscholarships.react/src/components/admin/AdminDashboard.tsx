import { useEffect } from "react";
import { UseUserAccountContext } from "../../contexts/UserAccountContext";
import { useNavigate } from "react-router-dom";

function AdminDashboard() {
  const context = UseUserAccountContext();
  const [cookie] = [context.userCookie];
  const navigation = useNavigate();

  useEffect(() => {
    if (!cookie.roles.includes("Admin")){
      navigation("/access-denied");
    }

  }, []);
  
  return (
      <>
        <div className="dashboard">
          <div className="row">
            <h1>BizKidz Scholarship Dashboard</h1>
          </div>

          <div className="row g-4">
            <div className="col-8">
              
            </div>
            
            <div className="col-4">
              
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

