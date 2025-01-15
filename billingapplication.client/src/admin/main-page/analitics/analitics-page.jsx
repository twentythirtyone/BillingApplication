import { TariffBarChart } from './tariff-graph.jsx';
import { TariffPieChart} from './new-users-graph.jsx';
import { PaymentLineChart} from './payment-line-chart.jsx';
import { useEffect } from 'react';

export const AnaliticsPage = () => {
    useEffect(() => {
        document.title = 'Статистика';
    })

    return (
        <div className="analitics-page">
            <div className='admin-graphics-container'>
                <div className='tariff-extras-graph-sect'>
                    <div>
                        <h2>Пользователи тарифов</h2>
                        <TariffBarChart urlAPI={'/billingapplication/tariff/user_count'} title={'Количество пользователей'}/>
                    
                    </div>
                    <div style={{ marginLeft:'20px' }}>
                    <h2>Приобритение доп. услуг</h2>
                    <TariffBarChart urlAPI={'/billingapplication/extras/month'} title={'Количество купивших услугу'}/>   
                    </div>
                </div>
                <div className='tariff-extras-graph-sect'>
                    <div>
                        <h2>Новые пользователи</h2>
                        <TariffPieChart urlAPI={'/billingapplication/subscribers/new_users/count'} title={'Количество новых пользователей'}/>
                    </div>
                    <div style={{ marginLeft:'20px'}}>
                        <h2>Оплата услуг</h2>
                        <PaymentLineChart/>   
                    </div>
                </div>
            </div>     
        </div>
    )
}