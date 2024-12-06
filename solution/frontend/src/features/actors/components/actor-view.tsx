import { Spinner } from '@/components/ui/spinner'

import { useActor } from '../api/get-actor'
import { Link, useNavigate } from 'react-router-dom'
import { paths } from '@/config/paths'
import { Button } from '@/components/ui/button'
import { ArrowLeft, Link as LinkUrl } from 'lucide-react'
import { UpdateActor } from './update-actor'

export const ActorView = ({ uniqueId }: { uniqueId: string }) => {
  const query = useActor({ uniqueId })
  const navigate = useNavigate()

  if (query.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const actor = query?.data?.data

  if (!actor) return null

  return (
    <>
      <div className="mt-6 flex flex-col ">
        <div className="flex justify-between">
          <Button size="sm" onClick={() => navigate(-1)} icon={<ArrowLeft className="size-4" />}>
            Back
          </Button>

          <UpdateActor uniqueId={uniqueId} />
        </div>
      </div>

      <div className="mt-6 flex flex-row space-y-8">
        <div>
          <p>
            <b>Name:</b> {actor.name}
          </p>

          <p className="mt-4">
            <b>Movies:</b>
            {actor.movies.map((movie, index) => (
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
