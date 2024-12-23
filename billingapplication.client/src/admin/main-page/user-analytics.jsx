import { UserInfo } from './user-info';
import { UserCharts } from './user-charts';
import { useParams } from 'react-router-dom';

export const UserAnalytics = () => {
  const { id } = useParams();

  return (
    <div className="user-analytics">
      <UserInfo userId={id} />
      <UserCharts userId={id} />
    </div>
  );
};