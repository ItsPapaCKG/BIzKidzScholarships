import { useState } from "react";
import { useUserPointsContext } from "../context";
import type { IUserPoints } from "../models/ViewModels";


function UserPoints() {
    const [points, setPoints] = useState<IUserPoints>({
        Total: 0,
        Entries: 0
    });

  return (
      <div>
          <div>
            <p>Points:</p>
              <p>{points.Total}</p>
          </div>

          <div>
              <p>Entries:</p>
              <p>{points.Entries}</p>
          </div>
      </div>
  );
}

export default UserPoints;