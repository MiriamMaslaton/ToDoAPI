import axios from 'axios';

axios.defaults.baseURL = process.env.REACT_APP_API_KEY
//"http://localhost:5135"
 
// setAuthorizationBearer();

// function saveAccessToken(accessToken){
//   localStorage.setItem("access_token", accessToken.token);
//   setAuthorizationBearer();
// }

// function setAuthorizationBearer(){
//   const accessToken = localStorage.getItem('access_token');
//   if(accessToken)
//     axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
// }

axios.interceptors.response.use(
  function(response){
   return response;
  },
  function(error){
    // if(error.response.staus === 401)
      //  return(window.location.href = "/login")
    console.log(error);
    return Promise.reject(error);
  }
)

export default {
  getTasks: async () => {
     const result = await axios.get(`/items`);
     return result.data;    
  },

  addTask: async(name)=>{
    const result = await axios.post(`/items`, {Name: name, IsComplete: false});
    // saveAccessToken(result.data);
    return result;
  },

  setCompleted: async(id, isComplete)=>{
    const result = await axios.put(`/items/${id}`, {IsComplete: isComplete });
    // saveAccessToken(result.data);
    return result;
  },

  deleteTask:async(id)=>{
    const result = await axios.delete(`/items/${id}`);
    // saveAccessToken(result.data)
    return result;
  }
};