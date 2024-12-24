import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

export const UserDetails = () => {
  const { id } = useParams();
  const [actions, setActions] = useState([]);

  useEffect(() => {
    const fetchUserActions = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get(`http://billing-app-server:5183/history/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setActions(response.data);
      } catch (error) {
        console.error('Error fetching user actions:', error);
      }
    };

    fetchUserActions();
  }, [id]);

  if (!actions.length) {
    return <div>Loading...</div>;
  }

  return (
    <div className="user-actions">
      <h1>User Actions</h1>
      <ul>
        {actions.map((action, index) => (
          <li key={index}>
            <p><strong>Type:</strong> {action.type}</p>
            <p><strong>From Subscriber ID:</strong> {action.data.fromSubscriberId}</p>
            <p><strong>To Phone Number:</strong> {action.data.toPhoneNumber}</p>
            <p><strong>Date:</strong> {new Date(action.data.date).toLocaleString()}</p>
            <p><strong>Duration:</strong> {action.data.duration} seconds</p>
            <p><strong>Price:</strong> {action.data.price}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};
