import { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export const UserTable = () => {
  const [users, setUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredUsers, setFilteredUsers] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    document.title = 'Клиенты';

    const fetchUsers = async () => {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get('/billingapplication/subscribers', {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        setUsers(response.data);
        setFilteredUsers(response.data);
      } catch (error) {
        console.error('Error fetching users:', error);
      }
    };

    fetchUsers();
  }, []);

  const handleSearch = (event) => {
    const value = event.target.value;
    setSearchTerm(value);
    setFilteredUsers(
      users.filter((user) =>
        user.passportInfo.fullName.toLowerCase().includes(value.toLowerCase()) ||
        user.number.toLowerCase().includes(value.toLowerCase())
      )
    );
  };

  const handleRowClick = (id) => {
    navigate(`/operator/monitoring/user-analytics/${id}`);
  };

  return (
    <div>
      <div className="user-table">
        <h1>Клиенты Alfa-Telecom</h1>
        <input
          type="text"
          placeholder="Поиск по ФИО или номеру телефона"
          value={searchTerm}
          onChange={handleSearch}
          className='user-table-search'
        />
        <div>
          <div className="custom-table-wrapper">
            <table className="custom-table">
              <thead>
                <tr>
                  <th className="col-id">ID</th>
                  <th className="col-name">ФИО</th>
                  <th>Номер</th>
                  <th>Тариф</th>
                  <th className="col-email">Email</th>
                </tr>
              </thead>
              <tbody>
                {filteredUsers.map((user) => (
                  <tr key={user.id} onClick={() => handleRowClick(user.id)}>
                    <td>{user.id}</td>
                    <td>{user.passportInfo.fullName}</td>
                    <td>{user.number}</td>
                    <td>{user.tariff.title}</td>
                    <td>{user.email}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
};