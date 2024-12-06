import { Spinner } from '@/components/ui/spinner'

import { useDirector } from '../api/get-director'
import { Link, useNavigate } from 'react-router-dom'
import { paths } from '@/config/paths'
import { Button } from '@/components/ui/button'
import { ArrowLeft, Link as LinkUrl } from 'lucide-react'
import { UpdateDirector } from '@/features/directors/components/update-director'

export const DirectorView = ({ uniqueId }: { uniqueId: string }) => {
  const query = useDirector({ uniqueId })
  const navigate = useNavigate()

  if (query.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const director = query?.data?.data

  if (!director) return null

  return (
    <>
      <div className="mt-6 flex flex-col ">
        <div className="flex justify-between">
          <Button size="sm" onClick={() => navigate(-1)} icon={<ArrowLeft className="size-4" />}>
            Back
          </Button>

          <UpdateDirector uniqueId={uniqueId} />
        </div>
      </div>

      <div className="mt-6 flex flex-row space-y-8">
        <div>
          <p>
            <b>Name:</b> {director.name}
          </p>

          <p className="mt-4">
            <b>Movies:</b>
            {director.movies.map((movie, index) => (
              <li key={index} className="ml-5 text-gray-700 list-none">
                <Link to={paths.app.movie.getHref(movie.uniqueId)} className="flex">
                  <LinkUrl className="size-4 mr-2" /> {movie.title}
                </Link>
              </li>
            ))}
          </p>
        </div>
      </div>
    </>
  )
}
