import { Link } from '@/components/ui/link'
import { paths } from '@/config/paths'
import { useNavigate } from 'react-router-dom'

export const NotFoundRoute = () => {
  const navigate = useNavigate()
  const goToMovies = () => navigate('/movies')
  return (
    <div className="mt-52 flex flex-col items-center font-semibold">
      <h1>404 - Not Found</h1>
      <p>Sorry, the page you are looking for does not exist.</p>
      <Link to={paths.app.root.path} replace>
        Go to Home
      </Link>

      <p onClick={goToMovies}>Go to Home</p>
    </div>
  )
}
