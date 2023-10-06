
const GetColor = (weather) =>

{
    let color = 'blue'; // Default color (you can set it to any color you prefer)

    if (weather === "overcast clouds") {
        color = 'orange';
    } else if (weather === "broken clouds") {
        color = 'purple';
    } else if (weather === "clear sky") {
        color = 'green';
    } else if (weather === "fog") {
        color = 'grey';
    } else if (weather === "few clouds") {
        color = 'blue';
    } else if (weather === "Light rain") {
        color = 'yellow';
    } else if (weather === "mist") {
        color = 'red';
    } else {
        color= 'brown'
    }

    return color;
        
        
}

const GetIcon = (weather) => {
    let imgSrc = '~/Content/Images/cloud2.png'; // Default color (you can set it to any color you prefer)

    if (weather === "overcast clouds") {
        color = 'orange';
    } else if (weather === "broken clouds") {
        color = 'purple';
    } else if (weather === "clear sky") {
        color = 'green';
    } else if (weather === "fog") {
        color = 'grey';
    } else if (weather === "few clouds") {
        color = 'blue';
    } else if (weather === "Light rain") {
        color = 'yellow';
    } else if (weather === "mist") {
        color = 'red';
    } else {
        color = 'brown'
    }

    return imgSrc;


}